﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IdealMall.DataAccess
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class IdealmallEntities : DbContext
    {
        public IdealmallEntities()
            : base("name=IdealmallEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CashAndCarry> CashAndCarrys { get; set; }
        public virtual DbSet<CreateAccount_Public> CreateAccount_Public { get; set; }
        public virtual DbSet<Login_Public> Login_Public { get; set; }
        public virtual DbSet<Login_Trade> Login_Trade { get; set; }
        public virtual DbSet<OfferProducts_Public> OfferProducts_Public { get; set; }
        public virtual DbSet<OfferProducts_Trade> OfferProducts_Trade { get; set; }
        public virtual DbSet<Retailshop> Retailshops { get; set; }
        public virtual DbSet<ShoppingList_Public> ShoppingList_Public { get; set; }
        public virtual DbSet<ShoppingList_Trade> ShoppingList_Trade { get; set; }
        public virtual DbSet<CreateAccount_Trade> CreateAccount_Trade { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<postcodelatlng> postcodelatlngs { get; set; }
    
        public virtual int f_sp_ImportExcel_Trade(string source, string sourceSheet, string destinationTable)
        {
            var sourceParameter = source != null ?
                new ObjectParameter("Source", source) :
                new ObjectParameter("Source", typeof(string));
    
            var sourceSheetParameter = sourceSheet != null ?
                new ObjectParameter("SourceSheet", sourceSheet) :
                new ObjectParameter("SourceSheet", typeof(string));
    
            var destinationTableParameter = destinationTable != null ?
                new ObjectParameter("DestinationTable", destinationTable) :
                new ObjectParameter("DestinationTable", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("f_sp_ImportExcel_Trade", sourceParameter, sourceSheetParameter, destinationTableParameter);
        }
    }
}