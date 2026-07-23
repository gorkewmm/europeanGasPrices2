using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddPartialUniqueIndexToUserOperationClaim : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserOperationClaims_UserId_OperationClaimId",
                table: "UserOperationClaims");

            migrationBuilder.DropIndex(
                name: "IX_OperationClaimPermissions_OperationClaimId_PermissionId",
                table: "OperationClaimPermissions");

            migrationBuilder.CreateIndex(
                name: "IX_UserOperationClaims_UserId_OperationClaimId",
                table: "UserOperationClaims",
                columns: new[] { "UserId", "OperationClaimId" },
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_OperationClaimPermissions_OperationClaimId_PermissionId",
                table: "OperationClaimPermissions",
                columns: new[] { "OperationClaimId", "PermissionId" },
                unique: true,
                filter: "\"IsDeleted\" = false");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserOperationClaims_UserId_OperationClaimId",
                table: "UserOperationClaims");

            migrationBuilder.DropIndex(
                name: "IX_OperationClaimPermissions_OperationClaimId_PermissionId",
                table: "OperationClaimPermissions");

            migrationBuilder.CreateIndex(
                name: "IX_UserOperationClaims_UserId_OperationClaimId",
                table: "UserOperationClaims",
                columns: new[] { "UserId", "OperationClaimId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OperationClaimPermissions_OperationClaimId_PermissionId",
                table: "OperationClaimPermissions",
                columns: new[] { "OperationClaimId", "PermissionId" },
                unique: true);
        }
    }
}
