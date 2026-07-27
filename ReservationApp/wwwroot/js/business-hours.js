// Business Hours card (Owner/Admin Settings pages) — disables a day's open/
// close time inputs while its "Closed" checkbox is ticked, and "Copy Monday
// to all days" fills every other row from Monday's current values.
(function () {
    const container = document.querySelector(".business-hours-table");
    if (!container) {
        return;
    }

    const rows = Array.from(container.querySelectorAll("[data-day-row]"));

    function toggleRow(row) {
        const closed = row.querySelector("[data-closed-checkbox]").checked;
        row.querySelector("[data-open-input]").disabled = closed;
        row.querySelector("[data-close-input]").disabled = closed;
    }

    rows.forEach(function (row) {
        toggleRow(row);
        row.querySelector("[data-closed-checkbox]").addEventListener("change", function () {
            toggleRow(row);
        });
    });

    const copyButton = document.getElementById("copy-monday-btn");
    if (copyButton && rows.length > 0) {
        copyButton.addEventListener("click", function () {
            const monday = rows[0];
            const mondayOpen = monday.querySelector("[data-open-input]").value;
            const mondayClose = monday.querySelector("[data-close-input]").value;
            const mondayClosed = monday.querySelector("[data-closed-checkbox]").checked;

            rows.slice(1).forEach(function (row) {
                row.querySelector("[data-open-input]").value = mondayOpen;
                row.querySelector("[data-close-input]").value = mondayClose;
                row.querySelector("[data-closed-checkbox]").checked = mondayClosed;
                toggleRow(row);
            });
        });
    }
})();
