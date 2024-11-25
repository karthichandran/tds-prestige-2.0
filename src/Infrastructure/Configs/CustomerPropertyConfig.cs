﻿// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT licence. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReProServices.Domain.Entities;

namespace ReProServices.Infrastructure.Configs
{
    public class CustomerPropertyConfig : IEntityTypeConfiguration<CustomerProperty>
    {
        public void Configure
            (EntityTypeBuilder<CustomerProperty> entity)
        {
            //entity.HasKey(p => 
            //    new { p.CustomerId, p.PropertyId }); //#A

            //-----------------------------
            //Relationships

            entity.HasOne(pt => pt.Customer)        //#C
                .WithMany(p => p.CustomerProperty)   //#C
                .HasForeignKey(pt => pt.CustomerId);//#C

            //entity.HasOne(pt => pt.Property)        //#C
            //    .WithMany(t => t.CustomerProperty)       //#C
            //    .HasForeignKey(pt => pt.PropertyId);//#C
        }

        /*Primary key settings**********************************************
        #A Here I use an anonymous object to define two (or more) properties to form a composite key. The order in which the properties appear in the anonymous object defines their order
        * ******************************************************/
        /*******************************************************
        #A This uses the names of the Book and Author primary keys to form its own, composite key
        #B I configure the one-to-many relationship from the Book to BookAuthor entity class
        #C ... and I then configure the other one-to-many relationship from the Author to the BookAuthor entity class
         * ****************************************************/
    }
}