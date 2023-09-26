#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace IdentityServer.Data.Migrations.IdentityServer.ConfigurationDb;

/// <inheritdoc />
public partial class InitialConfigurationDb : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            "Identity");

        migrationBuilder.CreateTable(
            "ApiResources",
            schema: "Identity",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Enabled = table.Column<bool>("boolean", nullable: false),
                Name = table.Column<string>("character varying(200)", maxLength: 200, nullable: false),
                DisplayName = table.Column<string>("character varying(200)", maxLength: 200, nullable: true),
                Description = table.Column<string>("character varying(1000)", maxLength: 1000, nullable: true),
                AllowedAccessTokenSigningAlgorithms = table.Column<string>("character varying(100)", maxLength: 100, nullable: true),
                ShowInDiscoveryDocument = table.Column<bool>("boolean", nullable: false),
                RequireResourceIndicator = table.Column<bool>("boolean", nullable: false),
                Created = table.Column<DateTime>("timestamp with time zone", nullable: false),
                Updated = table.Column<DateTime>("timestamp with time zone", nullable: true),
                LastAccessed = table.Column<DateTime>("timestamp with time zone", nullable: true),
                NonEditable = table.Column<bool>("boolean", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_ApiResources", x => x.Id); });

        migrationBuilder.CreateTable(
            "ApiScopes",
            schema: "Identity",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Enabled = table.Column<bool>("boolean", nullable: false),
                Name = table.Column<string>("character varying(200)", maxLength: 200, nullable: false),
                DisplayName = table.Column<string>("character varying(200)", maxLength: 200, nullable: true),
                Description = table.Column<string>("character varying(1000)", maxLength: 1000, nullable: true),
                Required = table.Column<bool>("boolean", nullable: false),
                Emphasize = table.Column<bool>("boolean", nullable: false),
                ShowInDiscoveryDocument = table.Column<bool>("boolean", nullable: false),
                Created = table.Column<DateTime>("timestamp with time zone", nullable: false),
                Updated = table.Column<DateTime>("timestamp with time zone", nullable: true),
                LastAccessed = table.Column<DateTime>("timestamp with time zone", nullable: true),
                NonEditable = table.Column<bool>("boolean", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_ApiScopes", x => x.Id); });

        migrationBuilder.CreateTable(
            "Clients",
            schema: "Identity",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Enabled = table.Column<bool>("boolean", nullable: false),
                ClientId = table.Column<string>("character varying(200)", maxLength: 200, nullable: false),
                ProtocolType = table.Column<string>("character varying(200)", maxLength: 200, nullable: false),
                RequireClientSecret = table.Column<bool>("boolean", nullable: false),
                ClientName = table.Column<string>("character varying(200)", maxLength: 200, nullable: true),
                Description = table.Column<string>("character varying(1000)", maxLength: 1000, nullable: true),
                ClientUri = table.Column<string>("character varying(2000)", maxLength: 2000, nullable: true),
                LogoUri = table.Column<string>("character varying(2000)", maxLength: 2000, nullable: true),
                RequireConsent = table.Column<bool>("boolean", nullable: false),
                AllowRememberConsent = table.Column<bool>("boolean", nullable: false),
                AlwaysIncludeUserClaimsInIdToken = table.Column<bool>("boolean", nullable: false),
                RequirePkce = table.Column<bool>("boolean", nullable: false),
                AllowPlainTextPkce = table.Column<bool>("boolean", nullable: false),
                RequireRequestObject = table.Column<bool>("boolean", nullable: false),
                AllowAccessTokensViaBrowser = table.Column<bool>("boolean", nullable: false),
                RequireDPoP = table.Column<bool>("boolean", nullable: false),
                DPoPValidationMode = table.Column<int>("integer", nullable: false),
                DPoPClockSkew = table.Column<TimeSpan>("interval", nullable: false),
                FrontChannelLogoutUri = table.Column<string>("character varying(2000)", maxLength: 2000, nullable: true),
                FrontChannelLogoutSessionRequired = table.Column<bool>("boolean", nullable: false),
                BackChannelLogoutUri = table.Column<string>("character varying(2000)", maxLength: 2000, nullable: true),
                BackChannelLogoutSessionRequired = table.Column<bool>("boolean", nullable: false),
                AllowOfflineAccess = table.Column<bool>("boolean", nullable: false),
                IdentityTokenLifetime = table.Column<int>("integer", nullable: false),
                AllowedIdentityTokenSigningAlgorithms = table.Column<string>("character varying(100)", maxLength: 100, nullable: true),
                AccessTokenLifetime = table.Column<int>("integer", nullable: false),
                AuthorizationCodeLifetime = table.Column<int>("integer", nullable: false),
                ConsentLifetime = table.Column<int>("integer", nullable: true),
                AbsoluteRefreshTokenLifetime = table.Column<int>("integer", nullable: false),
                SlidingRefreshTokenLifetime = table.Column<int>("integer", nullable: false),
                RefreshTokenUsage = table.Column<int>("integer", nullable: false),
                UpdateAccessTokenClaimsOnRefresh = table.Column<bool>("boolean", nullable: false),
                RefreshTokenExpiration = table.Column<int>("integer", nullable: false),
                AccessTokenType = table.Column<int>("integer", nullable: false),
                EnableLocalLogin = table.Column<bool>("boolean", nullable: false),
                IncludeJwtId = table.Column<bool>("boolean", nullable: false),
                AlwaysSendClientClaims = table.Column<bool>("boolean", nullable: false),
                ClientClaimsPrefix = table.Column<string>("character varying(200)", maxLength: 200, nullable: true),
                PairWiseSubjectSalt = table.Column<string>("character varying(200)", maxLength: 200, nullable: true),
                InitiateLoginUri = table.Column<string>("character varying(2000)", maxLength: 2000, nullable: true),
                UserSsoLifetime = table.Column<int>("integer", nullable: true),
                UserCodeType = table.Column<string>("character varying(100)", maxLength: 100, nullable: true),
                DeviceCodeLifetime = table.Column<int>("integer", nullable: false),
                CibaLifetime = table.Column<int>("integer", nullable: true),
                PollingInterval = table.Column<int>("integer", nullable: true),
                CoordinateLifetimeWithUserSession = table.Column<bool>("boolean", nullable: true),
                Created = table.Column<DateTime>("timestamp with time zone", nullable: false),
                Updated = table.Column<DateTime>("timestamp with time zone", nullable: true),
                LastAccessed = table.Column<DateTime>("timestamp with time zone", nullable: true),
                NonEditable = table.Column<bool>("boolean", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_Clients", x => x.Id); });

        migrationBuilder.CreateTable(
            "IdentityProviders",
            schema: "Identity",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Scheme = table.Column<string>("character varying(200)", maxLength: 200, nullable: false),
                DisplayName = table.Column<string>("character varying(200)", maxLength: 200, nullable: true),
                Enabled = table.Column<bool>("boolean", nullable: false),
                Type = table.Column<string>("character varying(20)", maxLength: 20, nullable: false),
                Properties = table.Column<string>("text", nullable: true),
                Created = table.Column<DateTime>("timestamp with time zone", nullable: false),
                Updated = table.Column<DateTime>("timestamp with time zone", nullable: true),
                LastAccessed = table.Column<DateTime>("timestamp with time zone", nullable: true),
                NonEditable = table.Column<bool>("boolean", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_IdentityProviders", x => x.Id); });

        migrationBuilder.CreateTable(
            "IdentityResources",
            schema: "Identity",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Enabled = table.Column<bool>("boolean", nullable: false),
                Name = table.Column<string>("character varying(200)", maxLength: 200, nullable: false),
                DisplayName = table.Column<string>("character varying(200)", maxLength: 200, nullable: true),
                Description = table.Column<string>("character varying(1000)", maxLength: 1000, nullable: true),
                Required = table.Column<bool>("boolean", nullable: false),
                Emphasize = table.Column<bool>("boolean", nullable: false),
                ShowInDiscoveryDocument = table.Column<bool>("boolean", nullable: false),
                Created = table.Column<DateTime>("timestamp with time zone", nullable: false),
                Updated = table.Column<DateTime>("timestamp with time zone", nullable: true),
                NonEditable = table.Column<bool>("boolean", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_IdentityResources", x => x.Id); });

        migrationBuilder.CreateTable(
            "ApiResourceClaims",
            schema: "Identity",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                ApiResourceId = table.Column<int>("integer", nullable: false),
                Type = table.Column<string>("character varying(200)", maxLength: 200, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ApiResourceClaims", x => x.Id);
                table.ForeignKey(
                    "FK_ApiResourceClaims_ApiResources_ApiResourceId",
                    x => x.ApiResourceId,
                    principalSchema: "Identity",
                    principalTable: "ApiResources",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "ApiResourceProperties",
            schema: "Identity",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                ApiResourceId = table.Column<int>("integer", nullable: false),
                Key = table.Column<string>("character varying(250)", maxLength: 250, nullable: false),
                Value = table.Column<string>("character varying(2000)", maxLength: 2000, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ApiResourceProperties", x => x.Id);
                table.ForeignKey(
                    "FK_ApiResourceProperties_ApiResources_ApiResourceId",
                    x => x.ApiResourceId,
                    principalSchema: "Identity",
                    principalTable: "ApiResources",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "ApiResourceScopes",
            schema: "Identity",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Scope = table.Column<string>("character varying(200)", maxLength: 200, nullable: false),
                ApiResourceId = table.Column<int>("integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ApiResourceScopes", x => x.Id);
                table.ForeignKey(
                    "FK_ApiResourceScopes_ApiResources_ApiResourceId",
                    x => x.ApiResourceId,
                    principalSchema: "Identity",
                    principalTable: "ApiResources",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "ApiResourceSecrets",
            schema: "Identity",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                ApiResourceId = table.Column<int>("integer", nullable: false),
                Description = table.Column<string>("character varying(1000)", maxLength: 1000, nullable: true),
                Value = table.Column<string>("character varying(4000)", maxLength: 4000, nullable: false),
                Expiration = table.Column<DateTime>("timestamp with time zone", nullable: true),
                Type = table.Column<string>("character varying(250)", maxLength: 250, nullable: false),
                Created = table.Column<DateTime>("timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ApiResourceSecrets", x => x.Id);
                table.ForeignKey(
                    "FK_ApiResourceSecrets_ApiResources_ApiResourceId",
                    x => x.ApiResourceId,
                    principalSchema: "Identity",
                    principalTable: "ApiResources",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "ApiScopeClaims",
            schema: "Identity",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                ScopeId = table.Column<int>("integer", nullable: false),
                Type = table.Column<string>("character varying(200)", maxLength: 200, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ApiScopeClaims", x => x.Id);
                table.ForeignKey(
                    "FK_ApiScopeClaims_ApiScopes_ScopeId",
                    x => x.ScopeId,
                    principalSchema: "Identity",
                    principalTable: "ApiScopes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "ApiScopeProperties",
            schema: "Identity",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                ScopeId = table.Column<int>("integer", nullable: false),
                Key = table.Column<string>("character varying(250)", maxLength: 250, nullable: false),
                Value = table.Column<string>("character varying(2000)", maxLength: 2000, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ApiScopeProperties", x => x.Id);
                table.ForeignKey(
                    "FK_ApiScopeProperties_ApiScopes_ScopeId",
                    x => x.ScopeId,
                    principalSchema: "Identity",
                    principalTable: "ApiScopes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "ClientClaims",
            schema: "Identity",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Type = table.Column<string>("character varying(250)", maxLength: 250, nullable: false),
                Value = table.Column<string>("character varying(250)", maxLength: 250, nullable: false),
                ClientId = table.Column<int>("integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ClientClaims", x => x.Id);
                table.ForeignKey(
                    "FK_ClientClaims_Clients_ClientId",
                    x => x.ClientId,
                    principalSchema: "Identity",
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "ClientCorsOrigins",
            schema: "Identity",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Origin = table.Column<string>("character varying(150)", maxLength: 150, nullable: false),
                ClientId = table.Column<int>("integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ClientCorsOrigins", x => x.Id);
                table.ForeignKey(
                    "FK_ClientCorsOrigins_Clients_ClientId",
                    x => x.ClientId,
                    principalSchema: "Identity",
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "ClientGrantTypes",
            schema: "Identity",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                GrantType = table.Column<string>("character varying(250)", maxLength: 250, nullable: false),
                ClientId = table.Column<int>("integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ClientGrantTypes", x => x.Id);
                table.ForeignKey(
                    "FK_ClientGrantTypes_Clients_ClientId",
                    x => x.ClientId,
                    principalSchema: "Identity",
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "ClientIdPRestrictions",
            schema: "Identity",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Provider = table.Column<string>("character varying(200)", maxLength: 200, nullable: false),
                ClientId = table.Column<int>("integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ClientIdPRestrictions", x => x.Id);
                table.ForeignKey(
                    "FK_ClientIdPRestrictions_Clients_ClientId",
                    x => x.ClientId,
                    principalSchema: "Identity",
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "ClientPostLogoutRedirectUris",
            schema: "Identity",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                PostLogoutRedirectUri = table.Column<string>("character varying(400)", maxLength: 400, nullable: false),
                ClientId = table.Column<int>("integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ClientPostLogoutRedirectUris", x => x.Id);
                table.ForeignKey(
                    "FK_ClientPostLogoutRedirectUris_Clients_ClientId",
                    x => x.ClientId,
                    principalSchema: "Identity",
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "ClientProperties",
            schema: "Identity",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                ClientId = table.Column<int>("integer", nullable: false),
                Key = table.Column<string>("character varying(250)", maxLength: 250, nullable: false),
                Value = table.Column<string>("character varying(2000)", maxLength: 2000, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ClientProperties", x => x.Id);
                table.ForeignKey(
                    "FK_ClientProperties_Clients_ClientId",
                    x => x.ClientId,
                    principalSchema: "Identity",
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "ClientRedirectUris",
            schema: "Identity",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                RedirectUri = table.Column<string>("character varying(400)", maxLength: 400, nullable: false),
                ClientId = table.Column<int>("integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ClientRedirectUris", x => x.Id);
                table.ForeignKey(
                    "FK_ClientRedirectUris_Clients_ClientId",
                    x => x.ClientId,
                    principalSchema: "Identity",
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "ClientScopes",
            schema: "Identity",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Scope = table.Column<string>("character varying(200)", maxLength: 200, nullable: false),
                ClientId = table.Column<int>("integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ClientScopes", x => x.Id);
                table.ForeignKey(
                    "FK_ClientScopes_Clients_ClientId",
                    x => x.ClientId,
                    principalSchema: "Identity",
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "ClientSecrets",
            schema: "Identity",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                ClientId = table.Column<int>("integer", nullable: false),
                Description = table.Column<string>("character varying(2000)", maxLength: 2000, nullable: true),
                Value = table.Column<string>("character varying(4000)", maxLength: 4000, nullable: false),
                Expiration = table.Column<DateTime>("timestamp with time zone", nullable: true),
                Type = table.Column<string>("character varying(250)", maxLength: 250, nullable: false),
                Created = table.Column<DateTime>("timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ClientSecrets", x => x.Id);
                table.ForeignKey(
                    "FK_ClientSecrets_Clients_ClientId",
                    x => x.ClientId,
                    principalSchema: "Identity",
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "IdentityResourceClaims",
            schema: "Identity",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                IdentityResourceId = table.Column<int>("integer", nullable: false),
                Type = table.Column<string>("character varying(200)", maxLength: 200, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_IdentityResourceClaims", x => x.Id);
                table.ForeignKey(
                    "FK_IdentityResourceClaims_IdentityResources_IdentityResourceId",
                    x => x.IdentityResourceId,
                    principalSchema: "Identity",
                    principalTable: "IdentityResources",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "IdentityResourceProperties",
            schema: "Identity",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                IdentityResourceId = table.Column<int>("integer", nullable: false),
                Key = table.Column<string>("character varying(250)", maxLength: 250, nullable: false),
                Value = table.Column<string>("character varying(2000)", maxLength: 2000, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_IdentityResourceProperties", x => x.Id);
                table.ForeignKey(
                    "FK_IdentityResourceProperties_IdentityResources_IdentityResour~",
                    x => x.IdentityResourceId,
                    principalSchema: "Identity",
                    principalTable: "IdentityResources",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            "IX_ApiResourceClaims_ApiResourceId_Type",
            schema: "Identity",
            table: "ApiResourceClaims",
            columns: new[] { "ApiResourceId", "Type" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ApiResourceProperties_ApiResourceId_Key",
            schema: "Identity",
            table: "ApiResourceProperties",
            columns: new[] { "ApiResourceId", "Key" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ApiResources_Name",
            schema: "Identity",
            table: "ApiResources",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ApiResourceScopes_ApiResourceId_Scope",
            schema: "Identity",
            table: "ApiResourceScopes",
            columns: new[] { "ApiResourceId", "Scope" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ApiResourceSecrets_ApiResourceId",
            schema: "Identity",
            table: "ApiResourceSecrets",
            column: "ApiResourceId");

        migrationBuilder.CreateIndex(
            "IX_ApiScopeClaims_ScopeId_Type",
            schema: "Identity",
            table: "ApiScopeClaims",
            columns: new[] { "ScopeId", "Type" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ApiScopeProperties_ScopeId_Key",
            schema: "Identity",
            table: "ApiScopeProperties",
            columns: new[] { "ScopeId", "Key" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ApiScopes_Name",
            schema: "Identity",
            table: "ApiScopes",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ClientClaims_ClientId_Type_Value",
            schema: "Identity",
            table: "ClientClaims",
            columns: new[] { "ClientId", "Type", "Value" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ClientCorsOrigins_ClientId_Origin",
            schema: "Identity",
            table: "ClientCorsOrigins",
            columns: new[] { "ClientId", "Origin" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ClientGrantTypes_ClientId_GrantType",
            schema: "Identity",
            table: "ClientGrantTypes",
            columns: new[] { "ClientId", "GrantType" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ClientIdPRestrictions_ClientId_Provider",
            schema: "Identity",
            table: "ClientIdPRestrictions",
            columns: new[] { "ClientId", "Provider" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ClientPostLogoutRedirectUris_ClientId_PostLogoutRedirectUri",
            schema: "Identity",
            table: "ClientPostLogoutRedirectUris",
            columns: new[] { "ClientId", "PostLogoutRedirectUri" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ClientProperties_ClientId_Key",
            schema: "Identity",
            table: "ClientProperties",
            columns: new[] { "ClientId", "Key" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ClientRedirectUris_ClientId_RedirectUri",
            schema: "Identity",
            table: "ClientRedirectUris",
            columns: new[] { "ClientId", "RedirectUri" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_Clients_ClientId",
            schema: "Identity",
            table: "Clients",
            column: "ClientId",
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ClientScopes_ClientId_Scope",
            schema: "Identity",
            table: "ClientScopes",
            columns: new[] { "ClientId", "Scope" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ClientSecrets_ClientId",
            schema: "Identity",
            table: "ClientSecrets",
            column: "ClientId");

        migrationBuilder.CreateIndex(
            "IX_IdentityProviders_Scheme",
            schema: "Identity",
            table: "IdentityProviders",
            column: "Scheme",
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_IdentityResourceClaims_IdentityResourceId_Type",
            schema: "Identity",
            table: "IdentityResourceClaims",
            columns: new[] { "IdentityResourceId", "Type" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_IdentityResourceProperties_IdentityResourceId_Key",
            schema: "Identity",
            table: "IdentityResourceProperties",
            columns: new[] { "IdentityResourceId", "Key" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_IdentityResources_Name",
            schema: "Identity",
            table: "IdentityResources",
            column: "Name",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            "ApiResourceClaims",
            "Identity");

        migrationBuilder.DropTable(
            "ApiResourceProperties",
            "Identity");

        migrationBuilder.DropTable(
            "ApiResourceScopes",
            "Identity");

        migrationBuilder.DropTable(
            "ApiResourceSecrets",
            "Identity");

        migrationBuilder.DropTable(
            "ApiScopeClaims",
            "Identity");

        migrationBuilder.DropTable(
            "ApiScopeProperties",
            "Identity");

        migrationBuilder.DropTable(
            "ClientClaims",
            "Identity");

        migrationBuilder.DropTable(
            "ClientCorsOrigins",
            "Identity");

        migrationBuilder.DropTable(
            "ClientGrantTypes",
            "Identity");

        migrationBuilder.DropTable(
            "ClientIdPRestrictions",
            "Identity");

        migrationBuilder.DropTable(
            "ClientPostLogoutRedirectUris",
            "Identity");

        migrationBuilder.DropTable(
            "ClientProperties",
            "Identity");

        migrationBuilder.DropTable(
            "ClientRedirectUris",
            "Identity");

        migrationBuilder.DropTable(
            "ClientScopes",
            "Identity");

        migrationBuilder.DropTable(
            "ClientSecrets",
            "Identity");

        migrationBuilder.DropTable(
            "IdentityProviders",
            "Identity");

        migrationBuilder.DropTable(
            "IdentityResourceClaims",
            "Identity");

        migrationBuilder.DropTable(
            "IdentityResourceProperties",
            "Identity");

        migrationBuilder.DropTable(
            "ApiResources",
            "Identity");

        migrationBuilder.DropTable(
            "ApiScopes",
            "Identity");

        migrationBuilder.DropTable(
            "Clients",
            "Identity");

        migrationBuilder.DropTable(
            "IdentityResources",
            "Identity");
    }
}