using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logic.Migrations
{
    public partial class EnableCDConPosts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Enable CDC on the database
            migrationBuilder.Sql("EXEC sys.sp_cdc_enable_db");

            // Enable CDC on the Posts table
            migrationBuilder.Sql(@"
                EXEC sys.sp_cdc_enable_table   
                @source_schema = N'dbo',  
                @source_name   = N'Posts', 
                @role_name     = NULL,
                @supports_net_changes = 1
            ");

            migrationBuilder.Sql(@"
                CREATE TABLE CdcTracking
                (
                    TableName NVARCHAR(128) PRIMARY KEY,
                    LastStartLsn BINARY(10) NOT NULL
                );
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Disable CDC on the Posts table.
            migrationBuilder.Sql(@"
                EXEC sys.sp_cdc_disable_table  
                @source_schema = N'dbo',  
                @source_name   = N'Posts',
                @capture_instance = 'dbo_Posts'
            ");
            migrationBuilder.Sql(@"
                DROP TABLE IF EXISTS CdcTrackings;
            ");

            // Disable CDC on the database
            migrationBuilder.Sql("EXEC sys.sp_cdc_disable_db");
        }
    }
}
