$(function () {
    var currentUser;
    $('#pagination-container').pagination({
        dataSource: $('#template').data('url'),
        locator: 'films',
        totalNumberLocator: function (response) {
            currentUser = response.currentUser; 
            return response.totalNumber;
        },
        pageSize: 6,
        showPageNumbers: true,
        callback: function (data, pagination) {
           simpleTemplating(data);  
        }
    });

    function simpleTemplating(data) {
        var $dataContainer = $('#films').empty();

        $.each(data, function (index, item) {
            var $templateClone = $('#template').children().clone(false, false);
            $templateClone.find('.name-film').html(item.name);
            $templateClone.find('.year-film').html(item.issueYear);
            $templateClone.find('.poster-film').attr("src", item.posterPath); 

            var btnUpdate = $templateClone.find('.btn-view');
            var hrefUpdate = btnUpdate.attr("href");
            btnUpdate.attr("href", hrefUpdate + "?idFilm=" + item.id);

            if (currentUser == item.userId) {
                var btnUpdate = $templateClone.find('.btn-update');
                var hrefUpdate = btnUpdate.attr("href");
                btnUpdate.attr("href", hrefUpdate + "?idFilm=" + item.id);
                btnUpdate.removeClass('d-none');
            }

            $dataContainer.append($templateClone);
        });
    }
});