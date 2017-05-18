
var method = "POST";

for (i = new Date().getFullYear()+1; i > 1920; i--)
{
    $('#yearpicker').append($('<option />').val(i).html(i));
}

$('#cast').DataTable({
    "order": [[1, "asc"]]
});

$('#awards').DataTable({
    "order": [[3, "asc"]]
});

$(document).ready(function() {
    $('#genreSelect').change(function() {
        $.ajax
        ({
            type: "PATCH",
            url: "/api/movie/genres",
            beforeSend: (xhr) => { xhr.setRequestHeader('Content-Type', 'application/json') },
            dataType: 'json',
            async: true,
            data: JSON.stringify({genres:$(this).val(),movieId:$('#castMovie').val()}),
            success: function(res) {
                console.log('genres updated');
            }, error: function(e) {
                console.log('error updating genres', e);
            }
        });
    })
})

function toggleMovieAwardModal() {
    $('#awardModal').modal('toggle');
}


if (getUrlParameter('save') === 'success') {
    showNotification('success', "Wijzigingen zijn succesvol doorgevoerd", false, 2000);
}

function addAward(notfMsg, method, dataObj, cb) {
    if (!notfMsg) notfMsg = "toegevoegd";
    if (!method) method = "POST";

    var movie = $('#castMovie').val();
    var person = $('#awardForm .select2search').val();
    var award = $('#movieAwardType').find(":selected").data('award');
    var awardType = $('#movieAwardType').find(":selected").data('type');
    var year = $('#yearpicker').val()
    var awardResult = $('#awardResult').val();

    if (!dataObj) dataObj = { personId: person, movieId: movie, type: awardType, name: award, year: year, result: awardResult };

    $.ajax
    ({
        type: method,
        url: "/api/movie/award",
        beforeSend: (xhr) => { xhr.setRequestHeader('Content-Type', 'application/json') },
        dataType: 'json',
        async: true,
        data: JSON.stringify(dataObj),
        success: function(res) {
            $("#awardModal").modal('toggle');
            if (cb) cb();
            showNotification('success', "Award: " + award + " | " + awardType +" succesvol "+notfMsg);
        }, error: function(e) {
            console.log('error', e);
            showNotification('success', "Award: " + award + " | " + awardType +" succesvol "+notfMsg);
        }
    });
}

function deleteAward(personName, name,type,personId,movieId,year) {
    if (confirm("Zeker weten dat je award: " + name + " | "+type+" van "+personName+" wil verwijderen?") === true) {
        addAward("verwijderd", "DELETE", { name: name, type: type, personId: personId, movieId: movieId, year: year });
    }
}

function toggleDirectorModal() {
    $('#directorModal').modal('toggle');
}

function addDirector() {

    var movie = $('#castMovie').val();
    var person = $('#directorForm .personSearch').val();

    $.ajax
    ({
        type: "POST",
        url: "/api/movie/director",
        beforeSend: (xhr) => { xhr.setRequestHeader('Content-Type', 'application/json') },
        dataType: 'json',
        async: true,
        data: JSON.stringify({ personId: person, movieId: movie}),
        success: function(res) {
            $("#directorModal").modal('toggle');
            showNotification('success', "Director: " + person + " succesvol toegevoegd");
        }
    });
}

function removeDirector(name, person, movie) {
    if (confirm("Zeker weten dat je director: " + name + " wil verwijderen?") === true) {
        $.ajax
        ({
            type: "DELETE",
            url: "/api/movie/director",
            beforeSend: (xhr) => { xhr.setRequestHeader('Content-Type', 'application/json') },
            dataType: 'json',
            async: true,
            data: JSON.stringify({ personId: person, movieId: movie }),
            success: function(res) {
                showNotification('success', "Director: " + person + " succesvol verwijderd");
            },
            error: function(e) {
                console.log('error', e);
                showNotification('success', "Director: " + name + " succesvol verwijderd");
            }
        });
    }
}



function getPoster(title) {
    $.get('https://api.themoviedb.org/3/search/movie?api_key=8f3879e8c91096e1e468cf6c539dc1e7&query=' +
        encodeURI(title),
        (res) => {
            setTimeout(() => {
                    if (res.results[0]) {
                        if (res.results[0].poster_path) {
                            $('#preview').attr('src',
                                'https://image.tmdb.org/t/p/w500/' + res.results[0].poster_path);
                            $('#preview-loader').fadeOut('slow',
                                () => {
                                    $('#preview').fadeIn('slow');
                                });
                        } else {
                            $('#preview-loader').fadeOut('slow',
                                () => {
                                    $('#preview').attr('src', '/images/no-poster.jpg');
                                    $('#preview').fadeIn('slow');
                                });

                        }
                        if (res.results[0].backdrop_path) {
                            //$('.container-fluid').css('background-image',"url('https://image.tmdb.org/t/p/original/"+ res.results[0].backdrop_path+"')");
                        }
                    } else {
                        $('#preview-loader').fadeOut('slow',
                            () => {
                                $('#preview').attr('src', '/images/no-poster.jpg');
                                $('#preview').fadeIn('slow');
                            });

                    }
                },
                200);

        });
}

$('#submitAddCast').click(function() {
    var person = $('#castmemberform .personSearch').val();
    var movie = $('#castMovie').val();
    var role = $('#castRole').val();
    $.ajax
    ({
        type: method,
        url: "/api/moviecast",
        beforeSend: (xhr) => { xhr.setRequestHeader('Content-Type', 'application/json') },
        dataType: 'json',
        async: true,
        data: JSON.stringify({ personId: person, movieId: movie, role: role }),
        success: function(res) {
            $("#memberModel").modal('toggle');
            showNotification('success', "Castmember: " + person + " succesvol aangepast / toegevoegd");
        }
    });
});

var urlperson = getUrlParameter("person");
if (urlperson) {
    $.get("/api/persons/" + urlperson,
        function(data) {
            $('.personSearch').append($("<option></option>")
                .attr("value", data.personId)
                .text(data.firstname + " " + data.lastname));
        });
    //$('#castPerson').val(urlperson);
    switch(getUrlParameter("editType")) {
        case 'director':
            $("#directorModal").modal('toggle');
            break;
        case 'person':
            $("#memberModel").modal('toggle');
            break;
    }

}

$('.select2search').select2({});

var searchUrl = "/api/movies/";
$("#previousPart").select2({
    minimumInputLength: 4,
    ajax: {
        delay: 250,
        url: function() {
            return searchUrl;
        },
        dataType: "json",
        type: "GET",
        data: function(params) {
            searchUrl = "/api/movies/search/" + params.term;
        },
        processResults: function(data) {
            return {
                results: $.map(data,
                    function(item) {
                        return {
                            text: "(" + item.publicationYear + ") " + item.title,
                            id: item.movieId
                        }
                    })
            };
        }
    }
});

$.fn.modal.Constructor.prototype.enforceFocus = function() {};
var searchUrlPersons = "/api/persons/";
$(".personSearch").select2({
    minimumInputLength: 4,
    ajax: {
        delay: 250,
        url: function() {
            return searchUrlPersons;
        },
        dataType: "json",
        type: "GET",
        data: function(params) {
            searchUrlPersons = "/api/persons/search/" + params.term;
        },
        processResults: function(data) {
            return {
                results: $.map(data,
                    function(item) {
                        return {
                            text: item.firstname + " " + item.lastname,
                            id: item.personId
                        }
                    })
            };
        }
    }
});


function newCastmember() {
    method = "POST";
    $("#castmemberform").trigger("reset");
    $("#memberModel").modal('toggle');
    console.log("setting method post");
}

function editCastMember(personId, person, role) {
    $('#castRole').val(role);
    $('.personSearch').append($("<option></option>")
        .attr("value", personId)
        .text(person));
    $("#memberModel").modal('toggle');
    method = "PATCH";
}

function deleteCastMember(personId, movieId, name) {
    if (confirm("Zeker weten dat je castmember: " + name + " wil verwijderen?") === true) {
        $.ajax({
            type: 'DELETE',
            url: "/api/moviecast",
            beforeSend: (xhr) => { xhr.setRequestHeader('Content-Type', 'application/json') },
            dataType: 'json',
            async: true,
            data: JSON.stringify({ personId: personId, movieId: movieId }),
            success: function(res) {
                //window.location = window.location.pathname;
                showNotification('success', "Castmember: " + name + " succesvol verwijderd");
            },
            error: function(e) {
                showNotification('success', "Castmember: " + name + " succesvol verwijderd");
            }
        });
    }
}

function showNotification(type, msg, refresh, timeout) {
    if (refresh == undefined) refresh = true;
    if (timeout == undefined) timeout = 1000;
    $('.crudNotification').addClass(type);
    $('.crudNotification').html(msg);
    $('.crudNotification').slideDown('fast',
        () => {
            $('#bodyContainer').css('marginTop', '78px');
            setTimeout(() => {
                    $('.crudNotification').slideUp('fast',
                        () => {
                            $('.crudNotification').removeClass('success');
                            $('#bodyContainer').css('marginTop', '50px');
                            //  window.location = window.location.pathname;
                            if(refresh === true) document.location.reload(true);
                        });
                },
                timeout);
        })
}

