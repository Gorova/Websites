function getList(id) {
    $.ajax({
        type: "Get",
        url: "/Archive/GetArchive?id=" + id,
        success: function (responce) {
           
            console.log(responce);
            var urls = Array();
            responce.forEach(function(item) {
                urls.push(item.Url);
            });
            var times = Array();
            responce.forEach(function(item) {
                times.push(item.MillisecondsOfLoading);
            });
            
            
            var ctx = document.getElementById("myChart");
            var myChart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: urls,
                    datasets: [{
                        label: '# of milliseconds',
                        data: times,
                        backgroundColor: [
                            'rgba(255, 99, 132, 0.2)',
                            'rgba(54, 162, 235, 0.2)',
                            'rgba(255, 206, 86, 0.2)',
                            'rgba(75, 192, 192, 0.2)',
                            'rgba(153, 102, 255, 0.2)',
                            'rgba(255, 159, 64, 0.2)'
                        ],
                        borderColor: [
                            'rgba(255,99,132,1)',
                            'rgba(54, 162, 235, 1)',
                            'rgba(255, 206, 86, 1)',
                            'rgba(75, 192, 192, 1)',
                            'rgba(153, 102, 255, 1)',
                            'rgba(255, 159, 64, 1)'
                        ],
                        borderWidth: 1
                    }]
                },
                options: {
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });

        }

    });

};

