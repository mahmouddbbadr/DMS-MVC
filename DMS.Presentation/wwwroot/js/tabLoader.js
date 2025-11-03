function initTabPage(config) {

    let {
        loadFoldersUrl,
        loadDocumentsUrl,
        pageSize,
        sortFieldFolders,
        sortFieldDocuments
    } = config;

    let sortOrderFol = "desc";
    let currentPageFol = 1;

    let sortOrderDoc = "desc";
    let currentPageDoc = 1;

    function getActiveTab() {
        return $(".nav-link.active").attr("data-bs-target").substring(1);
    }

    function loadFolders() {
        $.ajax({
            url: loadFoldersUrl,
            type: "GET",
            data: {
                searchTerm: $("#SearchVal").val(),
                pageNum: currentPageFol,
                pageSize: pageSize,
                sortField: sortFieldFolders,
                sortOrder: sortOrderFol
            },
            beforeSend: () =>
                $("#foldersContent").html('<div class="spinner-border text-primary"></div>'),
            success: result => $("#foldersContent").html(result),
            error: () =>
                $("#foldersContent").html('<div class="text-danger">Failed to load folders.</div>')
        });
    }

    function loadDocuments() {
        $.ajax({
            url: loadDocumentsUrl,
            type: "GET",
            data: {
                searchTerm: $("#SearchVal").val(),
                pageNum: currentPageDoc,
                pageSize: pageSize,
                sortField: sortFieldDocuments,
                sortOrder: sortOrderDoc
            },
            beforeSend: () =>
                $("#documentsContent").html('<div class="spinner-border text-primary"></div>'),
            success: result => $("#documentsContent").html(result),
            error: () =>
                $("#documentsContent").html('<div class="text-danger">Failed to load documents.</div>')
        });
    }

    function refresh() {
        getActiveTab() === "folders" ? loadFolders() : loadDocuments();
    }

    $(document).ready(function () {

        refresh();

        // Tab switch
        //$('button[data-bs-toggle="tab"]').on('shown.bs.tab', refresh);
        $('button[data-bs-toggle="tab"]').on('shown.bs.tab', function () {

            let target = $(this).attr("data-bs-target");

            if (target === "#folders") {
                $("#documentsContent").html("");
            } else {
                $("#foldersContent").html("");
            }

            refresh();
        });


        // Search
        $("#searchBtn").click(refresh);

        let typing;
        $("#SearchVal").on("input", function () {
            clearTimeout(typing);
            typing = setTimeout(refresh, 400);
        });

        // Sorting
        $(document).on("click", ".sort-link", function (e) {
            e.preventDefault();
            const field = $(this).data("field");

            if (getActiveTab() === "folders") {
                sortOrderFol = (sortFieldFolders === field && sortOrderFol === "asc") ? "desc" : "asc";
                sortFieldFolders = field;
                currentPageFol = 1;
                loadFolders();
            } else {
                sortOrderDoc = (sortFieldDocuments === field && sortOrderDoc === "asc") ? "desc" : "asc";
                sortFieldDocuments = field;
                currentPageDoc = 1;
                loadDocuments();
            }
        });

        // Pagination
        $(document).on("click", ".pagination a", function (e) {
            e.preventDefault();
            if (getActiveTab() === "folders") {
                currentPageFol = $(this).data("pagenum");
                loadFolders();
            } else {
                currentPageDoc = $(this).data("pagenum");
                loadDocuments();
            }
        });

      

        $(document).on("click", ".star-toggle-button", function (e) {
            e.preventDefault();
            const form = $(this).closest("form");

            Swal.fire({
                title: "Unstar this item?",
                icon: "question",
                showCancelButton: true,
                confirmButtonText: "Yes",
                cancelButtonText: "No"
            }).then(res => {
                if (res.isConfirmed) {
                    $.post(form.attr("action"), form.serialize())
                        .done(() => {

                            // Remove visually
                            form.closest(".document-row, tr, .card").fadeOut(300, function () {
                                $(this).remove();
                            });
                            setTimeout(refresh, 300);
                        })
                        .fail(() => Swal.fire("Error", "Unstar failed", "error"));
                }
            });
        });


        // Delete (move to trash)
        $(document).on("click", ".delete-button", function (e) {
            e.preventDefault();
            const form = $(this).closest("form");

            Swal.fire({
                title: "Move to trash?",
                icon: "warning",
                showCancelButton: true,
                confirmButtonText: "Yes",
                confirmButtonColor: "#d33"
            }).then(res => {
                if (res.isConfirmed) {
                    $.post(form.attr("action"), form.serialize())
                        .done(() => setTimeout(refresh, 300))
                        .fail(() => Swal.fire("Error", "Delete failed", "error"));
                }
            });
        });

        // Restore (Trash only)
        $(document).on("click", ".restore-button", function (e) {
            e.preventDefault();
            const form = $(this).closest("form");

            Swal.fire({
                title: "Restore?",
                icon: "question",
                showCancelButton: true,
                confirmButtonText: "Restore"
            }).then(res => {
                if (res.isConfirmed) {
                    $.post(form.attr("action"), form.serialize())
                        .done(() => setTimeout(refresh, 300))
                        .fail(() => Swal.fire("Error", "Restore failed", "error"));
                }
            });
        });

        // Delete Permanently
        $(document).on("click", ".delete-permanent-button", function (e) {
            e.preventDefault();
            const form = $(this).closest("form");

            Swal.fire({
                title: "Delete permanently?",
                icon: "error",
                showCancelButton: true,
                confirmButtonText: "Delete Forever",
                confirmButtonColor: "#b40a0a"
            }).then(res => {
                if (res.isConfirmed) {
                    $.post(form.attr("action"), form.serialize())
                        .done(() => setTimeout(refresh, 300))
                        .fail(() => Swal.fire("Error", "Delete failed", "error"));
                }
            });
        });

        // Edit
        //$(document).on("click", ".edit-doc-link", function (e) {
        //    e.preventDefault();
        //    let id = $(this).data("id");
        //    let returnUrl = encodeURIComponent(window.location.href);
        //    window.location.href = `/Document/Edit/${id}?returnUrl=${returnUrl}`;
        //});

        $(document).on("click", ".edit-doc-btn", function () {
            $("#editDocId").val($(this).data("id"));
            $("#editDocName").val($(this).data("name"));
            $("#editDocumentModal").modal("show");
        });

        $(document).on("submit", "#editDocForm", function (e) {
            e.preventDefault();

            let formData = new FormData(this);

            $.ajax({
                url: "/Document/EditModal",
                type: "POST",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    const modalEl = document.getElementById('editDocumentModal');
                    const modal = bootstrap.Modal.getInstance(modalEl) || new bootstrap.Modal(modalEl);
                    modal.hide();

                    Swal.fire({
                        icon: 'success',
                        title: 'Saved Successfully!',
                        timer: 1500,
                        showConfirmButton: false
                    });

                    refresh();
                },

                error: function () {
                    Swal.fire('Error!', 'Could not update document.', 'error');
                }
            });
        });

        $(document).on("click", ".folder-card", function (e) {

            // Ignore clicks on control buttons/icons
            if (
                $(e.target).closest(".star-toggle-button").length ||
                $(e.target).closest(".share-fol-link").length ||
                $(e.target).closest(".edit-fol-link").length ||
                $(e.target).closest(".delete-button").length
            ) {
                return;
            }

            const url = $(this).data("url");
            if (url) {
                window.location.href = url;
            }
        });
    });
}
