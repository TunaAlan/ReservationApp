// Restaurant photo gallery: hover the left/right edge of the hero image to cycle
// through photos in place, click the image to open a full-screen lightbox. One
// shared lightbox modal (#gallery-lightbox, in _Layout.cshtml) is reused by every
// gallery on the page instead of stamping out a lightbox per restaurant.
(function () {
    function initGalleries() {
        document.querySelectorAll(".restaurant-gallery").forEach(function (gallery) {
            if (gallery.dataset.galleryInit) return;
            gallery.dataset.galleryInit = "1";

            var images = JSON.parse(gallery.dataset.images);
            var index = 0;
            var heroImg = gallery.querySelector(".gallery-hero-img");
            var counter = gallery.querySelector(".gallery-counter");
            var prevZone = gallery.querySelector("[data-gallery-prev]");
            var nextZone = gallery.querySelector("[data-gallery-next]");

            function render() {
                heroImg.src = "/Restaurant_Img/" + images[index];
                if (counter) counter.textContent = (index + 1) + " / " + images.length;
            }

            function go(delta) {
                index = (index + delta + images.length) % images.length;
                render();
            }

            if (prevZone) {
                prevZone.addEventListener("click", function (e) {
                    e.stopPropagation();
                    go(-1);
                });
            }
            if (nextZone) {
                nextZone.addEventListener("click", function (e) {
                    e.stopPropagation();
                    go(1);
                });
            }

            heroImg.addEventListener("click", function () {
                openLightbox(images, index);
            });
        });
    }

    function openLightbox(images, startIndex) {
        var index = startIndex;
        var lightboxImg = document.getElementById("lightbox-img");
        var counter = document.getElementById("lightbox-counter");
        var prevBtn = document.getElementById("lightbox-prev");
        var nextBtn = document.getElementById("lightbox-next");
        if (!lightboxImg) return;

        function render() {
            lightboxImg.src = "/Restaurant_Img/" + images[index];
            counter.textContent = (index + 1) + " / " + images.length;
        }

        function go(delta) {
            index = (index + delta + images.length) % images.length;
            render();
        }

        prevBtn.onclick = function () { go(-1); };
        nextBtn.onclick = function () { go(1); };
        render();

        var modalEl = document.getElementById("gallery-lightbox");
        var modal = bootstrap.Modal.getOrCreateInstance(modalEl);
        modal.show();
    }

    document.addEventListener("DOMContentLoaded", initGalleries);
    // Galleries inside Bootstrap modals (restaurant details, etc.) exist in the DOM
    // from page load, but re-scanning on shown.bs.modal is a cheap no-op guard
    // (dataset.galleryInit) against any gallery markup injected later.
    document.addEventListener("shown.bs.modal", initGalleries);
})();
