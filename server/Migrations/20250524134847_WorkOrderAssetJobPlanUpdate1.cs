using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TWMSServer.Migrations
{
    /// <inheritdoc />
    public partial class WorkOrderAssetJobPlanUpdate1 : Migration
    {
        /// <inheritdoc />
        /// Shortened names for domain model fields. Added a status field for set-up page tables. 
        /// Reduced the length of CreatedBy and ModifiedBy fields
        /// (Stephen La Guerre, 2024-05-24))
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tblAssets_Classes_Attr_Maps",
                table: "tblAssets_Classes_Attr_Maps");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "tblAssets_Statuses");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "tblAssets_Statuses");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "tblAssets_Statuses");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "tblAssets_Statuses");

            migrationBuilder.RenameTable(
                name: "tblAssets_Classes_Attr_Maps",
                newName: "tblAssets_Classes_Attribute_Maps");

            migrationBuilder.RenameColumn(
                name: "SegmentTypeDescription",
                table: "tblSegment_Types",
                newName: "SegmentTypeDesc");

            migrationBuilder.RenameColumn(
                name: "JobPlanLineNo",
                table: "tblJob_Plans_Lines",
                newName: "JobPlanLineNum");

            migrationBuilder.RenameColumn(
                name: "Job_PlanId",
                table: "tblJob_Plans_Headers",
                newName: "JobPlanId");

            migrationBuilder.RenameColumn(
                name: "AssetStatusDescription",
                table: "tblAssets_Statuses",
                newName: "AssetStatusDesc");

            migrationBuilder.RenameColumn(
                name: "AssetStatusDescription",
                table: "tblAssets_Manufacturers",
                newName: "AssetStatusDesc");

            migrationBuilder.RenameColumn(
                name: "SourceDocument",
                table: "tblAssets_Failures",
                newName: "SourceDoc");

            migrationBuilder.RenameColumn(
                name: "FailureMode",
                table: "tblAssets_Failures",
                newName: "FailMode");

            migrationBuilder.RenameColumn(
                name: "FailureDate",
                table: "tblAssets_Failures",
                newName: "FailDate");

            migrationBuilder.RenameColumn(
                name: "FailureCause2",
                table: "tblAssets_Failures",
                newName: "FailCause2");

            migrationBuilder.RenameColumn(
                name: "FailureCause1",
                table: "tblAssets_Failures",
                newName: "FailCause1");

            migrationBuilder.RenameColumn(
                name: "CumulativeMTBF",
                table: "tblAssets_Failures",
                newName: "MTBF");

            migrationBuilder.RenameColumn(
                name: "AssetFailureId",
                table: "tblAssets_Failures",
                newName: "AssetFailId");

            migrationBuilder.RenameColumn(
                name: "UnitMeasure",
                table: "tblAssets_Attributes",
                newName: "Unit");

            migrationBuilder.RenameColumn(
                name: "AssetAttributeName",
                table: "tblAssets_Attributes",
                newName: "AssetAttrName");

            migrationBuilder.RenameColumn(
                name: "AssetAttributeId",
                table: "tblAssets_Attributes",
                newName: "AssetAttrId");

            migrationBuilder.RenameColumn(
                name: "VegetationPatrolFreq",
                table: "tblAssets",
                newName: "VegPatrolFreq");

            migrationBuilder.RenameColumn(
                name: "UltrasonicFreq",
                table: "tblAssets",
                newName: "UltraFreq");

            migrationBuilder.RenameColumn(
                name: "ThermographicFreq",
                table: "tblAssets",
                newName: "ThermoFreq");

            migrationBuilder.RenameColumn(
                name: "SerialNumber",
                table: "tblAssets",
                newName: "SerialNum");

            migrationBuilder.RenameColumn(
                name: "ModelNumber",
                table: "tblAssets",
                newName: "ModelNum");

            migrationBuilder.RenameColumn(
                name: "MaintenanceInterval",
                table: "tblAssets",
                newName: "MaintInterval");

            migrationBuilder.RenameColumn(
                name: "AssetGISId",
                table: "tblAssets",
                newName: "AssetGISNum");

            migrationBuilder.RenameColumn(
                name: "AerialInspectionFreq",
                table: "tblAssets",
                newName: "AerialInspectFreq");

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "tblZones",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "tblZones",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "tblZones",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "tblTx_Circuits",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "tblTx_Circuits",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "tblTx_Circuits",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "tblSubstations",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "tblSubstations",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "tblSubstations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "tblSegment_Types",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "tblSegment_Types",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "tblSegment_Points",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "tblSegment_Points",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "tblRings",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "tblRings",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "tblRings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "tblJob_Plans_Statuses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "tblJob_Plans_Statuses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "tblJob_Plans_Lines",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "tblJob_Plans_Lines",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "tblJob_Plans_Headers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "tblJob_Plans_Headers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "tblFeeders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "tblFeeders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "tblFeeders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "tblAssets_Manufacturers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "tblAssets_Manufacturers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "tblAssets_Manufacturers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "tblAssets_Manufacturers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "tblAssets_Manufacturers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "tblAssets_Failures",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "tblAssets_Failures",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "tblAssets_Classes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "tblAssets_Classes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "tblAssets_Classes",
                type: "bit",
                maxLength: 50,
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "tblAssets_Attributes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "tblAssets_Attributes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "tblAssets_Attributes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "tblAssets",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "tblAssets",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "tblAssets_Classes_Attribute_Maps",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "tblAssets_Classes_Attribute_Maps",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "tblAssets_Classes_Attribute_Maps",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblAssets_Classes_Attribute_Maps",
                table: "tblAssets_Classes_Attribute_Maps",
                column: "ClassAttrId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tblAssets_Classes_Attribute_Maps",
                table: "tblAssets_Classes_Attribute_Maps");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "tblZones");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "tblTx_Circuits");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "tblSubstations");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "tblRings");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "tblFeeders");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "tblAssets_Manufacturers");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "tblAssets_Manufacturers");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "tblAssets_Manufacturers");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "tblAssets_Manufacturers");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "tblAssets_Manufacturers");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "tblAssets_Classes");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "tblAssets_Attributes");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "tblAssets_Classes_Attribute_Maps");

            migrationBuilder.RenameTable(
                name: "tblAssets_Classes_Attribute_Maps",
                newName: "tblAssets_Classes_Attr_Maps");

            migrationBuilder.RenameColumn(
                name: "SegmentTypeDesc",
                table: "tblSegment_Types",
                newName: "SegmentTypeDescription");

            migrationBuilder.RenameColumn(
                name: "JobPlanLineNum",
                table: "tblJob_Plans_Lines",
                newName: "JobPlanLineNo");

            migrationBuilder.RenameColumn(
                name: "JobPlanId",
                table: "tblJob_Plans_Headers",
                newName: "Job_PlanId");

            migrationBuilder.RenameColumn(
                name: "AssetStatusDesc",
                table: "tblAssets_Statuses",
                newName: "AssetStatusDescription");

            migrationBuilder.RenameColumn(
                name: "AssetStatusDesc",
                table: "tblAssets_Manufacturers",
                newName: "AssetStatusDescription");

            migrationBuilder.RenameColumn(
                name: "SourceDoc",
                table: "tblAssets_Failures",
                newName: "SourceDocument");

            migrationBuilder.RenameColumn(
                name: "MTBF",
                table: "tblAssets_Failures",
                newName: "CumulativeMTBF");

            migrationBuilder.RenameColumn(
                name: "FailMode",
                table: "tblAssets_Failures",
                newName: "FailureMode");

            migrationBuilder.RenameColumn(
                name: "FailDate",
                table: "tblAssets_Failures",
                newName: "FailureDate");

            migrationBuilder.RenameColumn(
                name: "FailCause2",
                table: "tblAssets_Failures",
                newName: "FailureCause2");

            migrationBuilder.RenameColumn(
                name: "FailCause1",
                table: "tblAssets_Failures",
                newName: "FailureCause1");

            migrationBuilder.RenameColumn(
                name: "AssetFailId",
                table: "tblAssets_Failures",
                newName: "AssetFailureId");

            migrationBuilder.RenameColumn(
                name: "Unit",
                table: "tblAssets_Attributes",
                newName: "UnitMeasure");

            migrationBuilder.RenameColumn(
                name: "AssetAttrName",
                table: "tblAssets_Attributes",
                newName: "AssetAttributeName");

            migrationBuilder.RenameColumn(
                name: "AssetAttrId",
                table: "tblAssets_Attributes",
                newName: "AssetAttributeId");

            migrationBuilder.RenameColumn(
                name: "VegPatrolFreq",
                table: "tblAssets",
                newName: "VegetationPatrolFreq");

            migrationBuilder.RenameColumn(
                name: "UltraFreq",
                table: "tblAssets",
                newName: "UltrasonicFreq");

            migrationBuilder.RenameColumn(
                name: "ThermoFreq",
                table: "tblAssets",
                newName: "ThermographicFreq");

            migrationBuilder.RenameColumn(
                name: "SerialNum",
                table: "tblAssets",
                newName: "SerialNumber");

            migrationBuilder.RenameColumn(
                name: "ModelNum",
                table: "tblAssets",
                newName: "ModelNumber");

            migrationBuilder.RenameColumn(
                name: "MaintInterval",
                table: "tblAssets",
                newName: "MaintenanceInterval");

            migrationBuilder.RenameColumn(
                name: "AssetGISNum",
                table: "tblAssets",
                newName: "AssetGISId");

            migrationBuilder.RenameColumn(
                name: "AerialInspectFreq",
                table: "tblAssets",
                newName: "AerialInspectionFreq");

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "tblZones",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "tblZones",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "tblTx_Circuits",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "tblTx_Circuits",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "tblSubstations",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "tblSubstations",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "tblSegment_Types",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "tblSegment_Types",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "tblSegment_Points",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "tblSegment_Points",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "tblRings",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "tblRings",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "tblJob_Plans_Statuses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "tblJob_Plans_Statuses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "tblJob_Plans_Lines",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "tblJob_Plans_Lines",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "tblJob_Plans_Headers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "tblJob_Plans_Headers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "tblFeeders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "tblFeeders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "tblAssets_Statuses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "tblAssets_Statuses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "tblAssets_Statuses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "tblAssets_Statuses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "tblAssets_Failures",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "tblAssets_Failures",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "tblAssets_Classes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "tblAssets_Classes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "tblAssets_Attributes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "tblAssets_Attributes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "tblAssets",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "tblAssets",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "tblAssets_Classes_Attr_Maps",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "tblAssets_Classes_Attr_Maps",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblAssets_Classes_Attr_Maps",
                table: "tblAssets_Classes_Attr_Maps",
                column: "ClassAttrId");
        }
    }
}
