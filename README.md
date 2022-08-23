# Catalogo

## CLEAN ARCHITECTURE
-	ASP NET
-	Solution with clean architecture
- Use Dapper
- Unit Of Work

## Unit Of Work With Dapper

The objective is make a Unit Of Work, which can execute a `SaveChanges` from various Repositories, persistency the data.

With the Dependency Injection is managed a shared connection on the repositories.
In Catalogo.CrossCutting.IoC we could see this:
```C#
services
  .AddSingleton<IConfiguration>(configuration)
  .AddTransient<IConnectionFactory, ConnectionFactory>()
  .AddScoped<ICategoriaRepository, CategoriaRepository>()
  .AddScoped<IProdutoRepository, ProdutoRepository>()
  .AddScoped<IProdutoService, ProdutoService>()
  .AddScoped<ICategoriaService, CategoriaService>()
  
  // UoW
  .AddScoped<UnitOfWorkRepository>()
  .AddScoped<IUnitOfWorkRepository>(x => x.GetRequiredService<UnitOfWorkRepository>())
  .AddScoped<IUnitOfWork>(x => x.GetRequiredService<UnitOfWorkRepository>());
  ```
  
  The persistence of data execution is make in `Catalogo.Application.UoW.IUnitOfWork`, which have delivered to Services.
  The connection to access the data was inserted in the interface `Catalogo.Infrastructure.Context.UnitOfWork.Repository`, which is shared to Repositories.
  
  ```C#
namespace Catalogo.Infrastructure.Context
{
    /// <summary>
    /// Give a shared connection to repositorys
    /// </summary>
    public interface IUnitOfWorkRepository : IUnitOfWork
    {
        /// <summary>
        /// Avaliable connection
        /// </summary>
        IDbConnection Connection { get; }

        /// <summary>
        /// Avaliable Transaction
        /// </summary>
        IDbTransaction Transaction { get; }
    }
}

namespace Catalogo.Application.UoW
{
    /// <summary>
    /// Unit of work give a shared transactions to repositorys
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Identifier of unit
        /// </summary>
        Guid Identifier { get; }

        /// <summary>
        /// Necessary to create and open a new connection
        /// </summary>
        /// <returns>async result of <see cref="IUnitOfWork"/> opened</returns>
        Task<IUnitOfWork> OpenConnectionAsync();

        /// <summary>
        /// Creates a connection (if method <see cref="OpenConnectionAsync"/> haven't executed) and transaction
        /// </summary>
        /// <remarks>
        ///     <para>Starts a transaction to the repositorys</para>
        ///     <para>Use <see cref="SaveChangesAsync"/>> after execute</para>
        /// </remarks>
        /// <returns>async result of <see cref="IUnitOfWork"/> opened</returns>
        Task<IUnitOfWork> BeginTransactionAsync();

        /// <summary>
        /// Commits if is ok or roll back if throw a exception
        /// </summary>
        /// <returns>async</returns>
        Task SaveChangesAsync();
    }
}
  ```
  
  And a unique class implements a both (namespace `Catalogo.Infrastructure.Context`).
  
  ```C#
/// <summary>
/// Manage connections and transactions
/// </summary>
public class UnitOfWorkRepository : IUnitOfWorkRepository
{
      private DbConnection _connection { get; set; }
      public DbTransaction _transaction { get; set; }

      public IDbConnection Connection => _connection ?? throw new DataException("Connection is closed.");

      public IDbTransaction Transaction => _transaction;

      public Guid Identifier { get; } = Guid.NewGuid();

      private readonly IConnectionFactory _connectionFactory;

      public UnitOfWorkRepository(IConnectionFactory connectionFactory)
      {
          _connectionFactory = connectionFactory;
      }

      public async Task<IUnitOfWork> BeginTransactionAsync()
      {
          await OpenConnectionAsync();

          if (_transaction is null)
              _transaction = await  _connection.BeginTransactionAsync();

          return this;
      }

      public async Task SaveChangesAsync()
      {
          try
          {
              await _transaction?.CommitAsync();
          }
          catch
          {
              await _transaction?.RollbackAsync();
              throw;
          }
          finally
          {
              _transaction?.Dispose();
              _transaction = null;
          }
      }

      public void Dispose()
      {
          if (_transaction is not null)
          {
              _transaction.Dispose();
              _transaction = null;
          }

          if (_connection is not null)
          {
              _connection.Dispose();
              _connection = null;
          }
      }

      public async Task<IUnitOfWork> OpenConnectionAsync()
      {
          if (_connection is null)
          {
              _connection = _connectionFactory.CreateConn();
              await _connection.OpenAsync();
          }

          return this;
      }
}
  ```
  
  To use this implementation two parts needed.
  
  - Repository
  
  Needs a connection and transaction to execute the commands, and in all of the connections in DataBase should be use this way:
  
  ```C#
  [...]
  public class CategoriaRepository : RepositoryBase, ICategoriaRepository
  {
        private readonly IUnitOfWorkRepository _uoW;

        /// <summary>
        /// Connection to execute commands and querys
        /// </summary>
        protected IDbConnection _connection => _uoW.Connection;

        /// <summary>
        /// Shared execution
        /// </summary>
        protected IDbTransaction _transaction => _uoW.Transaction;

        public RepositoryBase(IUnitOfWorkRepository uoW)
        {
            _uoW = uoW;
        }

        public async Task<int> CreateAsync(Produto product)
          {
              return
                  await _connection.ExecuteAsync(
                      "INSERT INTO catalagodapper.produto (Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, IdCategoria) VALUES (@Nome, @Descricao, @Preco, @ImagemUrl, @Estoque, @DataCadastro, @IdCategoria);",
                      product,
                      _transaction
                  );
          }
  }
  [...]
  ```
  
  The Services manage a open connection to Repositories, having two ways, the simple execution, without transaction, or with it.

  ```C#
  public class CategoriaService : ICategoriaService
  {
      private readonly IUnitOfWork _uow;
      private ICategoriaRepository _categoryRepository;
      private readonly IMapper _mapper;

      public CategoriaService(
          IUnitOfWork uow,
          ICategoriaRepository categoryRepository,
          IMapper mapper)
      {
          _uow = uow;
          _categoryRepository = categoryRepository;
          _mapper = mapper;
      }

      public async Task Add(CategoriaDTO categoryDto)
      {
          var categoryEntity = _mapper.Map<Categoria>(categoryDto);

          categoryEntity.Validate();

          using (await _uow.BeginTransactionAsync())
          {
              if ((await _categoryRepository.GetByName(categoryEntity.Nome)) is not null)
                  throw new ConflictException($"Category with name {categoryEntity.Nome} already exists.");

              await _categoryRepository.CreateAsync(categoryEntity);
              await _uow.SaveChangesAsync();
          }
      }
  }
  ```
