using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TWMSServer.Migrations
{
    /// <inheritdoc />
    public partial class InitialWorkOrderAssetJobPlantOnTestServer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_Assets_tblWorkOrders_WorkOrderId",
                table: "tbl_Assets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_Zones",
                table: "tbl_Zones");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_Tx_Circuits",
                table: "tbl_Tx_Circuits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_Substations",
                table: "tbl_Substations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_Segment_Types",
                table: "tbl_Segment_Types");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_Segment_Points",
                table: "tbl_Segment_Points");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_Rings",
                table: "tbl_Rings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_Job_Plans_Statuses",
                table: "tbl_Job_Plans_Statuses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_Job_Plans_Lines",
                table: "tbl_Job_Plans_Lines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_Job_Plans_Headers",
                table: "tbl_Job_Plans_Headers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_Feeders",
                table: "tbl_Feeders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_Assets_Statuses",
                table: "tbl_Assets_Statuses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_Assets_Manufacturers",
                table: "tbl_Assets_Manufacturers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_Assets_Failures",
                table: "tbl_Assets_Failures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_Assets_Classes_Attr_Maps",
                table: "tbl_Assets_Classes_Attr_Maps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_Assets_Classes",
                table: "tbl_Assets_Classes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_Assets_Attributes",
                table: "tbl_Assets_Attributes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_Assets",
                table: "tbl_Assets");

            migrationBuilder.RenameTable(
                name: "tbl_Zones",
                newName: "tblZones");

            migrationBuilder.RenameTable(
                name: "tbl_Tx_Circuits",
                newName: "tblTx_Circuits");

            migrationBuilder.RenameTable(
                name: "tbl_Substations",
                newName: "tblSubstations");

            migrationBuilder.RenameTable(
                name: "tbl_Segment_Types",
                newName: "tblSegment_Types");

            migrationBuilder.RenameTable(
                name: "tbl_Segment_Points",
                newName: "tblSegment_Points");

            migrationBuilder.RenameTable(
                name: "tbl_Rings",
                newName: "tblRings");

            migrationBuilder.RenameTable(
                name: "tbl_Job_Plans_Statuses",
                newName: "tblJob_Plans_Statuses");

            migrationBuilder.RenameTable(
                name: "tbl_Job_Plans_Lines",
                newName: "tblJob_Plans_Lines");

            migrationBuilder.RenameTable(
                name: "tbl_Job_Plans_Headers",
                newName: "tblJob_Plans_Headers");

            migrationBuilder.RenameTable(
                name: "tbl_Feeders",
                newName: "tblFeeders");

            migrationBuilder.RenameTable(
                name: "tbl_Assets_Statuses",
                newName: "tblAssets_Statuses");

            migrationBuilder.RenameTable(
                name: "tbl_Assets_Manufacturers",
                newName: "tblAssets_Manufacturers");

            migrationBuilder.RenameTable(
                name: "tbl_Assets_Failures",
                newName: "tblAssets_Failures");

            migrationBuilder.RenameTable(
                name: "tbl_Assets_Classes_Attr_Maps",
                newName: "tblAssets_Classes_Attr_Maps");

            migrationBuilder.RenameTable(
                name: "tbl_Assets_Classes",
                newName: "tblAssets_Classes");

            migrationBuilder.RenameTable(
                name: "tbl_Assets_Attributes",
                newName: "tblAssets_Attributes");

            migrationBuilder.RenameTable(
                name: "tbl_Assets",
                newName: "tblAssets");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_Assets_WorkOrderId",
                table: "tblAssets",
                newName: "IX_tblAssets_WorkOrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblZones",
                table: "tblZones",
                column: "ZoneId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblTx_Circuits",
                table: "tblTx_Circuits",
                column: "TxCircuitId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblSubstations",
                table: "tblSubstations",
                column: "SubstationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblSegment_Types",
                table: "tblSegment_Types",
                column: "SegmentTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblSegment_Points",
                table: "tblSegment_Points",
                column: "SegmentPointId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblRings",
                table: "tblRings",
                column: "RingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblJob_Plans_Statuses",
                table: "tblJob_Plans_Statuses",
                column: "JobPlanStatusId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblJob_Plans_Lines",
                table: "tblJob_Plans_Lines",
                column: "Job_PlanLineId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblJob_Plans_Headers",
                table: "tblJob_Plans_Headers",
                column: "Job_PlanId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblFeeders",
                table: "tblFeeders",
                column: "FeederId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblAssets_Statuses",
                table: "tblAssets_Statuses",
                column: "AssetStatusId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblAssets_Manufacturers",
                table: "tblAssets_Manufacturers",
                column: "AssetStatusId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblAssets_Failures",
                table: "tblAssets_Failures",
                column: "AssetFailureId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblAssets_Classes_Attr_Maps",
                table: "tblAssets_Classes_Attr_Maps",
                column: "ClassAttrId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblAssets_Classes",
                table: "tblAssets_Classes",
                column: "AssetClassId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblAssets_Attributes",
                table: "tblAssets_Attributes",
                column: "AssetAttributeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblAssets",
                table: "tblAssets",
                column: "AssetId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblAssets_tblWorkOrders_WorkOrderId",
                table: "tblAssets",
                column: "WorkOrderId",
                principalTable: "tblWorkOrders",
                principalColumn: "WorkOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblAssets_tblWorkOrders_WorkOrderId",
                table: "tblAssets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tblZones",
                table: "tblZones");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tblTx_Circuits",
                table: "tblTx_Circuits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tblSubstations",
                table: "tblSubstations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tblSegment_Types",
                table: "tblSegment_Types");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tblSegment_Points",
                table: "tblSegment_Points");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tblRings",
                table: "tblRings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tblJob_Plans_Statuses",
                table: "tblJob_Plans_Statuses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tblJob_Plans_Lines",
                table: "tblJob_Plans_Lines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tblJob_Plans_Headers",
                table: "tblJob_Plans_Headers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tblFeeders",
                table: "tblFeeders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tblAssets_Statuses",
                table: "tblAssets_Statuses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tblAssets_Manufacturers",
                table: "tblAssets_Manufacturers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tblAssets_Failures",
                table: "tblAssets_Failures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tblAssets_Classes_Attr_Maps",
                table: "tblAssets_Classes_Attr_Maps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tblAssets_Classes",
                table: "tblAssets_Classes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tblAssets_Attributes",
                table: "tblAssets_Attributes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tblAssets",
                table: "tblAssets");

            migrationBuilder.RenameTable(
                name: "tblZones",
                newName: "tbl_Zones");

            migrationBuilder.RenameTable(
                name: "tblTx_Circuits",
                newName: "tbl_Tx_Circuits");

            migrationBuilder.RenameTable(
                name: "tblSubstations",
                newName: "tbl_Substations");

            migrationBuilder.RenameTable(
                name: "tblSegment_Types",
                newName: "tbl_Segment_Types");

            migrationBuilder.RenameTable(
                name: "tblSegment_Points",
                newName: "tbl_Segment_Points");

            migrationBuilder.RenameTable(
                name: "tblRings",
                newName: "tbl_Rings");

            migrationBuilder.RenameTable(
                name: "tblJob_Plans_Statuses",
                newName: "tbl_Job_Plans_Statuses");

            migrationBuilder.RenameTable(
                name: "tblJob_Plans_Lines",
                newName: "tbl_Job_Plans_Lines");

            migrationBuilder.RenameTable(
                name: "tblJob_Plans_Headers",
                newName: "tbl_Job_Plans_Headers");

            migrationBuilder.RenameTable(
                name: "tblFeeders",
                newName: "tbl_Feeders");

            migrationBuilder.RenameTable(
                name: "tblAssets_Statuses",
                newName: "tbl_Assets_Statuses");

            migrationBuilder.RenameTable(
                name: "tblAssets_Manufacturers",
                newName: "tbl_Assets_Manufacturers");

            migrationBuilder.RenameTable(
                name: "tblAssets_Failures",
                newName: "tbl_Assets_Failures");

            migrationBuilder.RenameTable(
                name: "tblAssets_Classes_Attr_Maps",
                newName: "tbl_Assets_Classes_Attr_Maps");

            migrationBuilder.RenameTable(
                name: "tblAssets_Classes",
                newName: "tbl_Assets_Classes");

            migrationBuilder.RenameTable(
                name: "tblAssets_Attributes",
                newName: "tbl_Assets_Attributes");

            migrationBuilder.RenameTable(
                name: "tblAssets",
                newName: "tbl_Assets");

            migrationBuilder.RenameIndex(
                name: "IX_tblAssets_WorkOrderId",
                table: "tbl_Assets",
                newName: "IX_tbl_Assets_WorkOrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_Zones",
                table: "tbl_Zones",
                column: "ZoneId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_Tx_Circuits",
                table: "tbl_Tx_Circuits",
                column: "TxCircuitId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_Substations",
                table: "tbl_Substations",
                column: "SubstationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_Segment_Types",
                table: "tbl_Segment_Types",
                column: "SegmentTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_Segment_Points",
                table: "tbl_Segment_Points",
                column: "SegmentPointId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_Rings",
                table: "tbl_Rings",
                column: "RingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_Job_Plans_Statuses",
                table: "tbl_Job_Plans_Statuses",
                column: "JobPlanStatusId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_Job_Plans_Lines",
                table: "tbl_Job_Plans_Lines",
                column: "Job_PlanLineId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_Job_Plans_Headers",
                table: "tbl_Job_Plans_Headers",
                column: "Job_PlanId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_Feeders",
                table: "tbl_Feeders",
                column: "FeederId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_Assets_Statuses",
                table: "tbl_Assets_Statuses",
                column: "AssetStatusId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_Assets_Manufacturers",
                table: "tbl_Assets_Manufacturers",
                column: "AssetStatusId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_Assets_Failures",
                table: "tbl_Assets_Failures",
                column: "AssetFailureId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_Assets_Classes_Attr_Maps",
                table: "tbl_Assets_Classes_Attr_Maps",
                column: "ClassAttrId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_Assets_Classes",
                table: "tbl_Assets_Classes",
                column: "AssetClassId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_Assets_Attributes",
                table: "tbl_Assets_Attributes",
                column: "AssetAttributeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_Assets",
                table: "tbl_Assets",
                column: "AssetId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_Assets_tblWorkOrders_WorkOrderId",
                table: "tbl_Assets",
                column: "WorkOrderId",
                principalTable: "tblWorkOrders",
                principalColumn: "WorkOrderId");
        }
    }
}
