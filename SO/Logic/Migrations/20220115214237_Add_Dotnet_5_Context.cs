using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logic.Migrations
{
    public partial class Add_Dotnet_5_Context : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "PostType",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        IsDeleted = table.Column<bool>(type: "bit", nullable: false),
            //        DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PostType", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Posts",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Score = table.Column<int>(type: "int", nullable: false),
            //        Tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        AcceptedAnswerId = table.Column<int>(type: "int", nullable: true),
            //        AnswerCount = table.Column<int>(type: "int", nullable: true),
            //        ClosedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        CommentCount = table.Column<int>(type: "int", nullable: true),
            //        CommunityOwnedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        CreationDate1 = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        FavoriteCount = table.Column<int>(type: "int", nullable: true),
            //        LastActivityDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        LastEditDate1 = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        LastEditorDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        LastEditorUserId = table.Column<int>(type: "int", nullable: true),
            //        OwnerUserId = table.Column<int>(type: "int", nullable: true),
            //        ParentId = table.Column<int>(type: "int", nullable: true),
            //        ViewCount = table.Column<int>(type: "int", nullable: false),
            //        PostTypeId = table.Column<int>(type: "int", nullable: true),
            //        CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        LastEditDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        IsDeleted = table.Column<bool>(type: "bit", nullable: false),
            //        DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Posts", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Posts_PostType_PostTypeId",
            //            column: x => x.PostTypeId,
            //            principalTable: "PostType",
            //            principalColumn: "Id");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Comments",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CreationDate1 = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        Score = table.Column<int>(type: "int", nullable: true),
            //        Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        UserId = table.Column<int>(type: "int", nullable: true),
            //        PostId = table.Column<int>(type: "int", nullable: true),
            //        CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        LastEditDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        IsDeleted = table.Column<bool>(type: "bit", nullable: false),
            //        DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Comments", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Comments_Posts_PostId",
            //            column: x => x.PostId,
            //            principalTable: "Posts",
            //            principalColumn: "Id");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "PostLink",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        PostId = table.Column<int>(type: "int", nullable: false),
            //        RelatedPostId = table.Column<int>(type: "int", nullable: false),
            //        LinkTypeId = table.Column<int>(type: "int", nullable: false),
            //        CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        IsDeleted = table.Column<bool>(type: "bit", nullable: false),
            //        DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PostLink", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_PostLink_Posts_PostId",
            //            column: x => x.PostId,
            //            principalTable: "Posts",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Vote",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        PostId = table.Column<int>(type: "int", nullable: false),
            //        UserId = table.Column<int>(type: "int", nullable: true),
            //        BountyAmount = table.Column<int>(type: "int", nullable: true),
            //        VoteTypeId = table.Column<int>(type: "int", nullable: false),
            //        CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        IsDeleted = table.Column<bool>(type: "bit", nullable: false),
            //        DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Vote", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Vote_Posts_PostId",
            //            column: x => x.PostId,
            //            principalTable: "Posts",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Comments_PostId",
            //    table: "Comments",
            //    column: "PostId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_PostLink_PostId",
            //    table: "PostLink",
            //    column: "PostId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Posts_PostTypeId",
            //    table: "Posts",
            //    column: "PostTypeId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Vote_PostId",
            //    table: "Vote",
            //    column: "PostId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "Comments");

            //migrationBuilder.DropTable(
            //    name: "PostLink");

            //migrationBuilder.DropTable(
            //    name: "Vote");

            //migrationBuilder.DropTable(
            //    name: "Posts");

            //migrationBuilder.DropTable(
            //    name: "PostType");
        }
    }
}
