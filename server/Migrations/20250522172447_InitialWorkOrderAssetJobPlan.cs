using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TWMSServer.Migrations
{
    /// <inheritdoc />
    public partial class InitialWorkOrderAssetJobPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblAssets");

            migrationBuilder.CreateTable(
                name: "tbl_Assets",
                columns: table => new
                {
                    AssetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SerialNumber = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    ModelNumber = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    AssetDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssetGISId = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    MaintenanceInterval = table.Column<int>(type: "int", nullable: true),
                    FromPoint = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    ToPoint = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    StreetName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Lvl1PatrolFreq = table.Column<int>(type: "int", nullable: true),
                    Lvl2PatrolFreq = table.Column<int>(type: "int", nullable: true),
                    Lvl3PatrolFreq = table.Column<int>(type: "int", nullable: true),
                    AerialInspectionFreq = table.Column<int>(type: "int", nullable: true),
                    ThermographicFreq = table.Column<int>(type: "int", nullable: true),
                    UltrasonicFreq = table.Column<int>(type: "int", nullable: true),
                    VegetationPatrolFreq = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkOrderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Assets", x => x.AssetId);
                    table.ForeignKey(
                        name: "FK_tbl_Assets_tblWorkOrders_WorkOrderId",
                        column: x => x.WorkOrderId,
                        principalTable: "tblWorkOrders",
                        principalColumn: "WorkOrderId");
                });

            migrationBuilder.CreateTable(
                name: "tbl_Assets_Attributes",
                columns: table => new
                {
                    AssetAttributeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetAttributeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UnitMeasure = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Assets_Attributes", x => x.AssetAttributeId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Assets_Classes",
                columns: table => new
                {
                    AssetClassId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetClassDesc = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Assets_Classes", x => x.AssetClassId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Assets_Classes_Attr_Maps",
                columns: table => new
                {
                    ClassAttrId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Assets_Classes_Attr_Maps", x => x.ClassAttrId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Assets_Failures",
                columns: table => new
                {
                    AssetFailureId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CumulativeMTBF = table.Column<long>(type: "bigint", nullable: false),
                    FailureDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SourceDocument = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FailureMode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FailureCause1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FailureCause2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Assets_Failures", x => x.AssetFailureId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Assets_Manufacturers",
                columns: table => new
                {
                    AssetStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetStatusDescription = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Assets_Manufacturers", x => x.AssetStatusId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Assets_Statuses",
                columns: table => new
                {
                    AssetStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetStatusDescription = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Assets_Statuses", x => x.AssetStatusId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Feeders",
                columns: table => new
                {
                    FeederId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeederName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Feeders", x => x.FeederId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Job_Plans_Headers",
                columns: table => new
                {
                    Job_PlanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobPlanDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Job_Plans_Headers", x => x.Job_PlanId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Job_Plans_Lines",
                columns: table => new
                {
                    Job_PlanLineId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobPlanId = table.Column<int>(type: "int", nullable: false),
                    JobPlanLineNo = table.Column<int>(type: "int", nullable: false),
                    JobPlanLineDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Job_Plans_Lines", x => x.Job_PlanLineId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Job_Plans_Statuses",
                columns: table => new
                {
                    JobPlanStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobPlanStatusName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Job_Plans_Statuses", x => x.JobPlanStatusId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Rings",
                columns: table => new
                {
                    RingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RingName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Rings", x => x.RingId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Segment_Points",
                columns: table => new
                {
                    SegmentPointId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SegmentPointName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Segment_Points", x => x.SegmentPointId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Segment_Types",
                columns: table => new
                {
                    SegmentTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SegmentTypeDescription = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Segment_Types", x => x.SegmentTypeId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Substations",
                columns: table => new
                {
                    SubstationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubstationName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Substations", x => x.SubstationId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Tx_Circuits",
                columns: table => new
                {
                    TxCircuitId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TxCircuitName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Tx_Circuits", x => x.TxCircuitId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Zones",
                columns: table => new
                {
                    ZoneId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ZoneName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Zones", x => x.ZoneId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Assets_WorkOrderId",
                table: "tbl_Assets",
                column: "WorkOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_Assets");

            migrationBuilder.DropTable(
                name: "tbl_Assets_Attributes");

            migrationBuilder.DropTable(
                name: "tbl_Assets_Classes");

            migrationBuilder.DropTable(
                name: "tbl_Assets_Classes_Attr_Maps");

            migrationBuilder.DropTable(
                name: "tbl_Assets_Failures");

            migrationBuilder.DropTable(
                name: "tbl_Assets_Manufacturers");

            migrationBuilder.DropTable(
                name: "tbl_Assets_Statuses");

            migrationBuilder.DropTable(
                name: "tbl_Feeders");

            migrationBuilder.DropTable(
                name: "tbl_Job_Plans_Headers");

            migrationBuilder.DropTable(
                name: "tbl_Job_Plans_Lines");

            migrationBuilder.DropTable(
                name: "tbl_Job_Plans_Statuses");

            migrationBuilder.DropTable(
                name: "tbl_Rings");

            migrationBuilder.DropTable(
                name: "tbl_Segment_Points");

            migrationBuilder.DropTable(
                name: "tbl_Segment_Types");

            migrationBuilder.DropTable(
                name: "tbl_Substations");

            migrationBuilder.DropTable(
                name: "tbl_Tx_Circuits");

            migrationBuilder.DropTable(
                name: "tbl_Zones");

            migrationBuilder.CreateTable(
                name: "tblAssets",
                columns: table => new
                {
                    AssetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkOrderId = table.Column<int>(type: "int", nullable: false),
                    AssetName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssetType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Manufacturer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModelNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblAssets", x => x.AssetId);
                    table.ForeignKey(
                        name: "FK_tblAssets_tblWorkOrders_WorkOrderId",
                        column: x => x.WorkOrderId,
                        principalTable: "tblWorkOrders",
                        principalColumn: "WorkOrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblAssets_WorkOrderId",
                table: "tblAssets",
                column: "WorkOrderId");
        }
    }
}
