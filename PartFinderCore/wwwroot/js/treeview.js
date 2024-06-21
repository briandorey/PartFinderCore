// treeview code
// 20 june 2024
// generate treeview from a nested ul li list and set node active if contains
// .treeactive class on li element.

function TreeView(rootElement) {
    // find all folders with sub folders and add plus arrows for toggling expansion
    var rootElement = document.querySelector(rootElement);

    // Ensure plusIcon is a valid DOM element
    var plusIcon = document.createElement('i');
    plusIcon.classList.add('bi', 'bi-caret-right-fill', 'tree-expand');

    rootElement.querySelectorAll("li").forEach(function (li) {
        if (li.querySelector("ul")) {
            // insert menu arrow
            li.insertAdjacentElement('afterbegin', plusIcon.cloneNode(true));
            // click code
            li.querySelector(".tree-expand").addEventListener("click", function () {
                var ul = this.parentElement.querySelector("ul");
                if (ul.style.display === "none" || ul.style.display === "") {
                    ul.style.display = "block";
                    // Animation complete.
                    var icon = this.parentElement.querySelector("i");
                    icon.classList.remove("bi-caret-right-fill", 'text-dark');
                    icon.classList.add("bi-caret-down-fill", 'text-dark');
                } else {
                    ul.style.display = "none";
                    // Animation complete.
                    var icon = this.parentElement.querySelector("i");
                    icon.classList.remove("bi-caret-down-fill", 'text-dark');
                    icon.classList.add("bi-caret-right-fill");
                }
            });
            // end click code

            // set active code
            li.querySelectorAll("li").forEach(function (lisub) {
                if (lisub.classList.contains("treeactive")) {
                    
                    // set text dark class if active
                    lisub.children[0].classList.add("text-dark");
                    // loop up the ul li list and set arrows to down for active folder tree
                    updateICaretRightToCaretDown(lisub);

                    var parentUl = lisub.closest('ul'); // Get the closest parent <ul>
                    while (parentUl && parentUl.id !== 'treeview') {
                        var ul = parentUl;
                        if (ul.style.display === "none" || ul.style.display === "") {
                            ul.style.display = "block";
                        }
                        element = parentUl.parentNode.closest('li'); // Get the closest parent <li> containing the <ul>
                        if (!element) break; // If no parent <li> found, exit the loop
                        parentUl = element.closest('ul'); // Get the closest parent <ul> again
                    }
                }
            });
        }
    });
}
// Function to update all "i" tags which contain the class "bi-caret-right-fill" from base level of selected li item
function updateICaretRightToCaretDown(element) {
    while (element) {
        // Find all child "i" elements with the class "bi-caret-right-fill" within the current element
        var iTags = element.querySelectorAll('i.bi-caret-right-fill');

        iTags.forEach(function (iTag) {
            iTag.classList.remove('bi-caret-right-fill', 'text-dark');
            iTag.classList.add('bi-caret-down-fill', 'text-dark');
        });

        // Move up to the parent "li" element
        if (element.tagName === 'LI') {
            element = element.closest('ul').parentElement.closest('li');
        } else if (element.tagName === 'UL' && element.id !== 'treeview') {
            element = element.parentElement.closest('li');
        } else {
            element = null;
        }
    }
}

TreeView("#treeview");