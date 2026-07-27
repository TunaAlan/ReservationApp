// Reservation receipt modal — "Download PDF" triggers the browser's native
// print-to-PDF dialog, "Download Image" rasterizes the receipt with html2canvas.
document.addEventListener("click", function (event) {
    const printButton = event.target.closest("[data-receipt-print]");
    if (printButton) {
        const modal = printButton.closest(".modal");
        const source = modal ? modal.querySelector(".receipt-print-area") : null;
        if (source) {
            printReceipt(source);
        }
        return;
    }

    const imageButton = event.target.closest("[data-receipt-download-image]");
    if (imageButton) {
        const targetId = imageButton.getAttribute("data-receipt-download-image");
        const target = document.getElementById(targetId);
        if (!target || typeof html2canvas === "undefined") {
            return;
        }

        html2canvas(target, { backgroundColor: "#ffffff", scale: 2 }).then(function (canvas) {
            const link = document.createElement("a");
            link.download = (imageButton.getAttribute("data-receipt-filename") || "reservation-receipt") + ".png";
            link.href = canvas.toDataURL("image/png");
            link.click();
        });
    }
});

// Printing the receipt in place fails because Bootstrap's modal transform
// creates a new containing block, clipping/mispositioning the print-only
// content. Instead, clone the receipt into a plain body-level element (no
// modal chrome involved) and print only that.
function printReceipt(sourceEl) {
    const printRoot = document.createElement("div");
    printRoot.id = "receipt-print-root";
    printRoot.innerHTML = sourceEl.innerHTML;
    document.body.appendChild(printRoot);
    document.body.classList.add("printing-receipt");

    function cleanup() {
        printRoot.remove();
        document.body.classList.remove("printing-receipt");
        window.removeEventListener("afterprint", cleanup);
    }
    window.addEventListener("afterprint", cleanup);

    window.print();
}
