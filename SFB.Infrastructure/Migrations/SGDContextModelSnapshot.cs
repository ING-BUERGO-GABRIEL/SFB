﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SGD.Infrastructure.Contexts;

#nullable disable

namespace SGD.Infrastructure.Migrations
{
    [DbContext(typeof(SGDContext))]
    partial class SGDContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("SGD.Infrastructure.Entities.AMS.EUser", b =>
                {
                    b.Property<string>("NameUser")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Email")
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("varchar(1000)");

                    b.Property<string>("Phone")
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<string>("UserType")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("varchar(2)");

                    b.HasKey("NameUser");

                    b.ToTable("AMS_User", (string)null);
                });

            modelBuilder.Entity("SGD.Infrastructure.Entities.AMS.EUserGroup", b =>
                {
                    b.Property<string>("NameUser")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("NroGroup")
                        .HasColumnType("int");

                    b.HasKey("NameUser", "NroGroup");

                    b.HasIndex("NroGroup");

                    b.ToTable("AMS_UserGroup", (string)null);
                });

            modelBuilder.Entity("SGD.Infrastructure.Entities.CMS.EGroup", b =>
                {
                    b.Property<int>("NroGroup")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("NroGroup"));

                    b.Property<string>("CodModule")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("varchar(3)");

                    b.Property<string>("Description")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.HasKey("NroGroup");

                    b.HasIndex("CodModule");

                    b.ToTable("CMS_Group", (string)null);
                });

            modelBuilder.Entity("SGD.Infrastructure.Entities.CMS.EGroupOption", b =>
                {
                    b.Property<int>("NroGroup")
                        .HasColumnType("int");

                    b.Property<string>("CodOption")
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.HasKey("NroGroup", "CodOption");

                    b.HasIndex("CodOption");

                    b.ToTable("CMS_GroupOption", (string)null);
                });

            modelBuilder.Entity("SGD.Infrastructure.Entities.CMS.EModule", b =>
                {
                    b.Property<string>("CodModule")
                        .HasMaxLength(3)
                        .HasColumnType("varchar(3)");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Icon")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("CodModule");

                    b.ToTable("CMS_Module", (string)null);
                });

            modelBuilder.Entity("SGD.Infrastructure.Entities.CMS.EOptionMenu", b =>
                {
                    b.Property<string>("CodOption")
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("CodGruOption")
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Description")
                        .HasMaxLength(300)
                        .HasColumnType("varchar(300)");

                    b.Property<string>("Icon")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.Property<string>("Route")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("CodOption");

                    b.HasIndex("CodGruOption");

                    b.ToTable("CMS_OptionMenu", (string)null);
                });

            modelBuilder.Entity("SGD.Infrastructure.Entities.AMS.EUserGroup", b =>
                {
                    b.HasOne("SGD.Infrastructure.Entities.AMS.EUser", "User")
                        .WithMany("UserGroups")
                        .HasForeignKey("NameUser")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SGD.Infrastructure.Entities.CMS.EGroup", "Group")
                        .WithMany("UserGroups")
                        .HasForeignKey("NroGroup")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SGD.Infrastructure.Entities.CMS.EGroup", b =>
                {
                    b.HasOne("SGD.Infrastructure.Entities.CMS.EModule", "Module")
                        .WithMany("Groups")
                        .HasForeignKey("CodModule")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Module");
                });

            modelBuilder.Entity("SGD.Infrastructure.Entities.CMS.EGroupOption", b =>
                {
                    b.HasOne("SGD.Infrastructure.Entities.CMS.EOptionMenu", "OptionMenu")
                        .WithMany("GroupOptions")
                        .HasForeignKey("CodOption")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SGD.Infrastructure.Entities.CMS.EGroup", "Group")
                        .WithMany("GroupOptions")
                        .HasForeignKey("NroGroup")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("OptionMenu");
                });

            modelBuilder.Entity("SGD.Infrastructure.Entities.CMS.EOptionMenu", b =>
                {
                    b.HasOne("SGD.Infrastructure.Entities.CMS.EOptionMenu", null)
                        .WithMany()
                        .HasForeignKey("CodGruOption")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("SGD.Infrastructure.Entities.AMS.EUser", b =>
                {
                    b.Navigation("UserGroups");
                });

            modelBuilder.Entity("SGD.Infrastructure.Entities.CMS.EGroup", b =>
                {
                    b.Navigation("GroupOptions");

                    b.Navigation("UserGroups");
                });

            modelBuilder.Entity("SGD.Infrastructure.Entities.CMS.EModule", b =>
                {
                    b.Navigation("Groups");
                });

            modelBuilder.Entity("SGD.Infrastructure.Entities.CMS.EOptionMenu", b =>
                {
                    b.Navigation("GroupOptions");
                });
#pragma warning restore 612, 618
        }
    }
}
