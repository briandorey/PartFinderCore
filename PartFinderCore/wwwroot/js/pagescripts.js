document.addEventListener('DOMContentLoaded', function () {

    var rows = document.querySelectorAll('.table tr');
    rows.forEach(function (row) {
        row.addEventListener('click', function () {
            var link = row.querySelector('a');
            if (link) {
                var href = link.getAttribute('href');
                if (href) {
                    window.location = href;
                }
            }
        });
    });

});