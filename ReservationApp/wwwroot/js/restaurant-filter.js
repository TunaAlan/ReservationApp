// Restaurant list search/category/city filter bar — submits automatically
// instead of requiring Enter or a Filter click. Typing debounces (waits for
// a pause) before submitting; changing a select submits immediately, since
// that's already a deliberate, discrete action.
(function () {
    const DEBOUNCE_MS = 400;

    document.querySelectorAll(".filter-bar").forEach(function (form) {
        const searchInput = form.querySelector(".filter-bar-input");
        const selects = form.querySelectorAll("select");
        let debounceTimer = null;

        if (searchInput) {
            // Auto-submit is a full page reload (plain GET form, no fetch/AJAX
            // rewrite), which drops focus entirely. Restoring it — with the
            // cursor at the end of what was typed — is what makes that reload
            // feel like "live" filtering instead of "click back into the box
            // after every pause."
            if (searchInput.value) {
                searchInput.focus();
                const end = searchInput.value.length;
                searchInput.setSelectionRange(end, end);
            }

            searchInput.addEventListener("input", function () {
                clearTimeout(debounceTimer);
                debounceTimer = setTimeout(function () {
                    form.requestSubmit();
                }, DEBOUNCE_MS);
            });
        }

        selects.forEach(function (select) {
            select.addEventListener("change", function () {
                clearTimeout(debounceTimer);
                form.requestSubmit();
            });
        });
    });
})();
