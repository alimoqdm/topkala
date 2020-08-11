var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    
    dataTable = $('#myTable').DataTable({
        responsive: true,
        "ajax": {
            "url": "/Admin/Categories/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "name", "autoWidth": true },
            { "data": "level", "autoWidth": true },
            {
                "data": "parent.name", "autoWidth": true,
                "defaultContent": "<i>-</i>"},

            {
                "data": "categoryId",
                "render": function (data) {
                    return `<div class="text-center">
                                <a href="/Admin/categories/Edit/${data}" class='btn btn-success text-white' style='cursor:pointer; width:100px;'>
                                    <i class='far fa-edit'></i> Edit
                                </a>
                                &nbsp;
                                <a onclick=Delete(${data}) class='btn btn-danger text-white' style='cursor:pointer; width:100px;'>
                                    <i class='far fa-trash-alt'></i> Delete
                                </a>
                            </div>
                            `;
                }, "autoWidth": true
            }
        ],
        "language": {
            "emptyTable":"No records found."
        },
        "width":"100%"
    });
}





function Delete(id) {
    swal({
        title: "Are you sure you want to delete",
        text: "!You will not be able to restore the content",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, delete it!",
        closeOnconfirm: true
    }, function () {
        $.ajax({
            type: 'GET',
            url: "/Admin/Categories/Delete/" + id,
            dataType: 'json',
            success: function (data) {
                if (data.success) {
                    toastr.success(data.message);
                    dataTable.ajax.reload();

                }
                else {
                    toastr.error(data.message);
                }
            },
            error: function (error) {
                toastr.error("U cant delet this ");

            }

        });
    });
}
