#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Domain.Seedwork;
using Infrastructure.Entities;
using Repository.NHibernate;
using Repository.UnitOfWork;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Specification;

#endregion

namespace Service
{
    public class EntityService<T> : EntityServiceBase<T> where T : class, IAggregateRoot
    {
        #region Constructor

        private readonly IUnitOfWork _unitOfWork;

        public EntityService()
            : this(new UnitOfWork())
        {
        }

        public EntityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            var nhibernateUnitOfWork = _unitOfWork as INhibernateUnitOfWork;
            if (nhibernateUnitOfWork == null)
            {
                throw new ArgumentNullException("INhibernateUnitOfWork");
            }
            Repository = new Repository<T>(nhibernateUnitOfWork);
            //Repository = new NHibernateRepository<T>();
        }

        #endregion

        #region Overrides of EntityServiceBase<T>

        #region Create

        public override T Add(T entity)
        {
            return Repository.Add(entity);
        }

        //public override IList<T> Add(IList<T> entities)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion

        #region Update

        public override void Update(T entity)
        {
            Repository.Update(entity);
        }

        #endregion

        #region Delete

        public override void Delete(T entity)
        {
            Repository.Delete(entity);
        }

        #endregion

        #region Select

        //public virtual T Find(int id)
        //{
        //    var repository = (Repository as Repository<T>);
        //    if (repository == null)
        //    {
        //        throw new ArgumentNullException("Repository");
        //    }
        //    return repository.Find(id);
        //}

        /// <summary>
        /// Get specific entity by query (Expression)
        /// </summary>
        /// <param name="query">Expression to evaluate</param>
        /// <returns>Item matching query</returns>
        public virtual T GetBy(Expression<Func<T, bool>> query)
        {
            var repository = (Repository as Repository<T>);
            if (repository == null)
            {
                throw new ArgumentNullException("Repository");
            }
            return repository.GetBy(query);
        }

        public override T GetById(int entityId)
        {
            return Repository.GetById(entityId);
        }

        public override T LoadById(int entityId)
        {
            return Repository.LoadById(entityId);
        }

        public override IQueryable<T> GetAll()
        {
            return Repository.GetAll();
        }

        //public IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        //{
        //    var repository = (Repository as Repository<T>);
        //    if (repository == null)
        //    {
        //        throw new ArgumentNullException("Repository");
        //    }
        //    return repository.AllIncluding(includeProperties);

        //}

        public override List<T> GetList()
        {
            return GetAll().ToList();
        }


        public override IQueryable<T> AllMatching(ISpecification<T> specification)
        {
            return Repository.AllMatching(specification);
        }

        #endregion

        #endregion

        #region Unit Of Work support

        public virtual void AddEntity(T entity)
        {
            var repository = (Repository as Repository<T>);
            if (repository == null)
            {
                throw new ArgumentNullException("Repository");
            }
            repository.AddEntity(entity);
        }


        /// <summary>
        /// Delete item 
        /// </summary>
        /// <param name="entity">Item to delete</param>
        /// <exception cref="ArgumentNullException"><paramref name="entity" /> is <c>null</c>.</exception>
        public virtual void DeletEntity(T entity)
        {
            var repository = (Repository as Repository<T>);
            if (repository == null)
            {
                throw new ArgumentNullException("Repository");
            }
            repository.DeleteEntity(entity);
        }

        /// <summary>
        /// Update entity into the repository
        /// </summary>
        /// <param name="entity">Item with changes</param>
        /// <exception cref="ArgumentNullException"><paramref name="entity" /> is <c>null</c>.</exception>
        public virtual void UpdateEntity(T entity)
        {
            var repository = (Repository as Repository<T>);
            if (repository == null)
            {
                throw new ArgumentNullException("Repository");
            }
            repository.UpdateEntity(entity);
        }

        #endregion
    }
}