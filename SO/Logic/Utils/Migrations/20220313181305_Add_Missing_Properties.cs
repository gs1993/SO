using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logic.Migrations
{
    public partial class Add_Missing_Properties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Badges",
                nullable: false,
                defaultValue: false);

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Badges",
                newName: "CreateDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateDate",
                table: "Badges",
                nullable: true,
                defaultValue: null);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteDate",
                table: "Badges",
                nullable: true,
                defaultValue: null);


            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Comments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "Comments",
                newName: "CreateDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateDate",
                table: "Comments",
                nullable: true,
                defaultValue: null);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteDate",
                table: "Comments",
                nullable: true,
                defaultValue: null);


            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "LinkTypes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "LinkTypes",
                nullable: false,
                defaultValue: new DateTime(2000, 01, 01));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateDate",
                table: "LinkTypes",
                nullable: true,
                defaultValue: null);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteDate",
                table: "LinkTypes",
                nullable: true,
                defaultValue: null);


            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PostLinks",
                nullable: false,
                defaultValue: false);

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "PostLinks",
                newName: "CreateDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateDate",
                table: "PostLinks",
                nullable: true,
                defaultValue: null);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteDate",
                table: "PostLinks",
                nullable: true,
                defaultValue: null);


            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Posts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "Posts",
                newName: "CreateDate");

            migrationBuilder.RenameColumn(
                name: "LastEditDate",
                table: "Posts",
                newName: "LastUpdateDate");
            
            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteDate",
                table: "Posts",
                nullable: true,
                defaultValue: null);


            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PostTypes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "PostTypes",
                nullable: false,
                defaultValue: new DateTime(2000, 01, 01));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateDate",
                table: "PostTypes",
                nullable: true,
                defaultValue: null);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteDate",
                table: "PostTypes",
                nullable: true,
                defaultValue: null);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
