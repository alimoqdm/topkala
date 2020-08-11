/*--Telegram Channel : @Web_Pixel--*/

$(document).ready(function () {

    winres();

    function winres() {
        setTimeout(function () {
            if ($(window).width() >= 992) {
                var ww = $(window).width();
                var sw = document.getElementById("sidebar-wrapper").offsetWidth;
                var cw = ww - sw;
                $("#content-wrapper").css("width", cw);
            } else {
                $("#content-wrapper").css("width", $(window).width());
            }
        }, 510);
    }
    $(window).resize(winres);

    const $button = document.querySelector('#sidebar-toggle');
    const $wrapper = document.querySelector('#wrapper');

    $button.addEventListener('click', (e) => {
        $wrapper.classList.toggle('toggled');
        setTimeout(function () {
            if ($(window).width() >= 992) {
                var ww = $(window).width();
                var sw = document.getElementById("sidebar-wrapper").offsetWidth;
                var cw = ww - sw;
                $("#content-wrapper").css("width", cw);
            }
        }, 510);
    });


    var ctx = document.getElementById('myChart').getContext('2d');
    var myChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: ['Hong Kong', 'Macau', 'Japan', 'Switzerland', 'Spain', 'Singapore'],
            datasets: [{
                label: 'Life expectancy',
                data: [84.308, 84.188, 84.118, 83.706, 83.5, 83.468],
                backgroundColor: [
                    'rgba(216, 27, 96, 0.6)',
                    'rgba(3, 169, 244, 0.6)',
                    'rgba(255, 152, 0, 0.6)',
                    'rgba(29, 233, 182, 0.6)',
                    'rgba(156, 39, 176, 0.6)',
                    'rgba(84, 110, 122, 0.6)'
                ],
                borderColor: [
                    'rgba(216, 27, 96, 1)',
                    'rgba(3, 169, 244, 1)',
                    'rgba(255, 152, 0, 1)',
                    'rgba(29, 233, 182, 1)',
                    'rgba(156, 39, 176, 1)',
                    'rgba(84, 110, 122, 1)'
                ],
                borderWidth: 1
            }]
        },
        options: {
            legend: {
                display: false
            },
            title: {
                display: true,
                text: 'Life Expectancy by Country',
                position: 'top',
                fontSize: 16,
                padding: 20
            },
            scales: {
                yAxes: [{
                    ticks: {
                        min: 75
                    }
                }]
            }
        }
    });









    var ctx = document.getElementById('myChart2').getContext('2d');
    var myChart = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: ['Dairy', 'Fruits', 'Lean meats', 'Vegetables', 'Whole grains'],
            datasets: [{
                data: [27.92, 17.53, 14.94, 26.62, 12.99],
                backgroundColor: ['#e91e63', '#00e676', '#ff5722', '#1e88e5', '#ffd600'],
                borderWidth: 0.5,
                borderColor: '#ddd'
            }]
        },
        options: {
            title: {
                display: true,
                text: 'Recommended Daily Diet',
                position: 'top',
                fontSize: 16,
                fontColor: '#111',
                padding: 20
            },
            legend: {
                display: true,
                position: 'bottom',
                labels: {
                    boxWidth: 20,
                    fontColor: '#111',
                    padding: 15
                }
            },
            tooltips: {
                enabled: false
            },
            plugins: {
                datalabels: {
                    color: '#111',
                    textAlign: 'center',
                    font: {
                        lineHeight: 1.6
                    },
                    formatter: function (value, ctx) {
                        return ctx.chart.data.labels[ctx.dataIndex] + '\n' + value + '%';
                    }
                }
            }
        }
    });


 
    $(".dropdown-btn").click(function(){
        $("#dropdown-container").slideToggle("slow");
      });

      $("#flip").click(function(){
        $("#panel").slideToggle("slow");
      });

    $(".dropdown-btn").click(function (e) {
        

        $("#dropdownContent").toggleClass(".test");
        // var dropdownContent = document.getElementById("dropdown-container");
        // if ($(dropdownContent).css("opacity", "0") &&   $(dropdownContent).css("height", "0")) {
        //     $(dropdownContent).css("opacity", "1");
        //     $(dropdownContent).css("height", "auto");
        // }
        // else {
        //     $(dropdownContent).css("opacity", "0");
        //     $(dropdownContent).css("height", "0");
        //     console.log("kir")
        // }
    });
 



});




// var dropdown = document.getElementsByClassName("dropdown-btn");
// var i;

// for (i = 0; i < dropdown.length; i++) {
//     console.log(i);
//   dropdown[i].addEventListener("click", function() {
//   this.classList.toggle("active");
//   var dropdownContent = this.nextElementSibling;
//   if (dropdownContent.style.opacity === 0) {
//   dropdownContent.style.opacity = "1";
//   dropdownContent.style.height = "auto";
//   } else {
//     dropdownContent.style.opacity === "0";
//     dropdownContent.style.height === "0";
//   }
//   });
// }