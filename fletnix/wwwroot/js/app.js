managerApp = angular.module('fletnix',[], function($interpolateProvider){

});

managerApp.controller('movie.edit.controller',[ '$scope', '$http', function($scope, $http) {
    $scope.state = {};
    $scope.setState = function(state) {
    }
/* $http.get("welcome.htm")
        .then(function(response) {
            $scope.myWelcome = response.data;
        });*/

/*$.get('https://api.themoviedb.org/3/search/movie?api_key=8f3879e8c91096e1e468cf6c539dc1e7&query=' +
        encodeURI('@(Model.Title)'),
        (res) => {
            setTimeout(() => {
                    if (res.results[0]) {
                        if (res.results[0].poster_path) {
                            $('#preview').attr('src',
                                'https://image.tmdb.org/t/p/w500/' + res.results[0].poster_path);
                            $('#preview-loader').fadeOut('slow',() => {
                                $('#preview').fadeIn('slow');
                            });
                        } else {
                            $('#preview-loader').fadeOut('slow',() => {
                                $('#preview').attr('src','/images/no-poster.jpg');
                                $('#preview').fadeIn('slow');
                            });

                        }
                        if (res.results[0].backdrop_path) {
                            //$('.container-fluid').css('background-image',"url('https://image.tmdb.org/t/p/original/"+ res.results[0].backdrop_path+"')");
                        }
                    } else {
                        $('#preview-loader').fadeOut('slow', () => {
                            $('#preview').attr('src','/images/no-poster.jpg');
                            $('#preview').fadeIn('slow');
                        });

                    }
                },
                200);

        });*/
}]);