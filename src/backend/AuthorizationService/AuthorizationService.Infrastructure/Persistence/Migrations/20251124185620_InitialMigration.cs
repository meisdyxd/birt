using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthorizationService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "resource_types",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()", comment: "Идентификатор"),
                    code = table.Column<string>(type: "text", nullable: false, comment: "Код"),
                    name = table.Column<string>(type: "text", nullable: false, comment: "Наименование"),
                    description = table.Column<string>(type: "text", nullable: true, comment: "Описание")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_resource_type_id", x => x.id);
                },
                comment: "Типы ресурсов");

            migrationBuilder.CreateTable(
                name: "subjects",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()", comment: "Идентификатор"),
                    user_external_id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Внешний идентификатор пользователя"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()", comment: "Дата создания"),
                    last_updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()", comment: "Дата последнего обновления")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_subject_id", x => x.id);
                },
                comment: "Субъекты(пользователи)");

            migrationBuilder.CreateTable(
                name: "permissions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()", comment: "Идентификатор"),
                    code = table.Column<string>(type: "text", nullable: false, comment: "Код"),
                    name = table.Column<string>(type: "text", nullable: false, comment: "Наименование"),
                    description = table.Column<string>(type: "text", nullable: true, comment: "Описание"),
                    resource_type_id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Тип ресурса")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_permission_id", x => x.id);
                    table.ForeignKey(
                        name: "FK_permissions_resource_types_resource_type_id",
                        column: x => x.resource_type_id,
                        principalTable: "resource_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Разрешения");

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()", comment: "Идентификатор"),
                    code = table.Column<string>(type: "text", nullable: false, comment: "Код"),
                    name = table.Column<string>(type: "text", nullable: false, comment: "Наименование"),
                    description = table.Column<string>(type: "text", nullable: true, comment: "Описание"),
                    resource_type_id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Тип ресурса"),
                    Level = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_id", x => x.id);
                    table.ForeignKey(
                        name: "FK_roles_resource_types_resource_type_id",
                        column: x => x.resource_type_id,
                        principalTable: "resource_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Роли");

            migrationBuilder.CreateTable(
                name: "resources",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()", comment: "Идентификатор"),
                    resource_type_id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Идентификатор типа ресурса"),
                    external_id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Внешний доменный идентификатор"),
                    owner_subject_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "Идентификатор владельца")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_resource_id", x => x.id);
                    table.ForeignKey(
                        name: "FK_resources_resource_types_resource_type_id",
                        column: x => x.resource_type_id,
                        principalTable: "resource_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_resources_subjects_owner_subject_id",
                        column: x => x.owner_subject_id,
                        principalTable: "subjects",
                        principalColumn: "id");
                },
                comment: "Ресурсы");

            migrationBuilder.CreateTable(
                name: "role_permissions",
                columns: table => new
                {
                    role_id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Идентификатор роли"),
                    permission_id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Идентификатор разрешения")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_id_permission_id", x => new { x.role_id, x.permission_id });
                    table.ForeignKey(
                        name: "FK_role_permissions_permissions_permission_id",
                        column: x => x.permission_id,
                        principalTable: "permissions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_role_permissions_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Связь ролей с разрешениями");

            migrationBuilder.CreateTable(
                name: "subject_roles",
                columns: table => new
                {
                    subject_id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Идентификатор субъекта(пользователя)"),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Идентификатор роли"),
                    resource_id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Идентификатор ресурса"),
                    valid_from = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()", comment: "Дата выдачи роли"),
                    valid_to = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата деактивирования роли")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_subject_id_role_id_resource_id", x => new { x.subject_id, x.role_id, x.resource_id });
                    table.ForeignKey(
                        name: "FK_subject_roles_resources_resource_id",
                        column: x => x.resource_id,
                        principalTable: "resources",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_subject_roles_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_subject_roles_subjects_subject_id",
                        column: x => x.subject_id,
                        principalTable: "subjects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Связь субъекта с ролями");

            migrationBuilder.CreateIndex(
                name: "idx_code_permissions",
                table: "permissions",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_permissions_resource_type_id",
                table: "permissions",
                column: "resource_type_id");

            migrationBuilder.CreateIndex(
                name: "idx_code_resource_types",
                table: "resource_types",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_resources_owner_subject_id",
                table: "resources",
                column: "owner_subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_resources_resource_type_id",
                table: "resources",
                column: "resource_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_role_permissions_permission_id",
                table: "role_permissions",
                column: "permission_id");

            migrationBuilder.CreateIndex(
                name: "idx_code_roles",
                table: "roles",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_roles_resource_type_id",
                table: "roles",
                column: "resource_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_subject_roles_resource_id",
                table: "subject_roles",
                column: "resource_id");

            migrationBuilder.CreateIndex(
                name: "IX_subject_roles_role_id",
                table: "subject_roles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "idx_user_external_id_subjects",
                table: "subjects",
                column: "user_external_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "role_permissions");

            migrationBuilder.DropTable(
                name: "subject_roles");

            migrationBuilder.DropTable(
                name: "permissions");

            migrationBuilder.DropTable(
                name: "resources");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "subjects");

            migrationBuilder.DropTable(
                name: "resource_types");
        }
    }
}
