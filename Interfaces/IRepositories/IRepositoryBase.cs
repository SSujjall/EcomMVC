﻿namespace EcomSiteMVC.Interfaces.IRepositories
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task Delete(T entity);
        Task<T>? GetById(object id);
        Task SaveChangesAsync();
    }
}