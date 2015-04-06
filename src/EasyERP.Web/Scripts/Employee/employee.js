﻿/*员工信息*/
$(document).ready(function () {

    var dataSource = new kendo.data.DataSource({
        transport: {
            read: {
                url: "employee/employeeList/1",
                type: "get",
                dataType: "json"
            }
        },
        schema: {
            data : "employeesList",
            model: {
                fields: {
                    Id: { editable: false, nullable: true },
                    FullName: { type: "string" },
                    Sex: { type: "string" },
                    IdNumber: { type: "string" },
                    Address: { type: "string" },
                    CellPhone: { type: "string" },
                    Education: { type: "string" },
                    NativePlace: { type: "string" }
                }
            }
        }
    });

    $("#employeesList").kendoGrid({
        dataSource: dataSource,
        height: 400,
        selectable: "multiple",
        pageable: {
            refresh: true,
        },
        columns: [
            { field: "FullName", title: "姓名", template: '<a href="/Employee/Edit/${Id}" target="_blank">${FullName}</a>' },
            { field: "Sex", title: "性别" },
            { field: "CellPhone", title: "手机号" },
            { field: "NativePlace", title: "籍贯" },
            { field: "IdNumber", title: "身份证号" },
            { field: "Address", title: "现住地址" },
            { field: "Department", title: "部门" },
            { field: "Education", title: "学历" }
        ],
    });
});