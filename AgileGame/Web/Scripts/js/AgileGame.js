

function renderComulativeFlow(div, title, subtitle, categories, yTitles, valueSuffix, series) {
    Highcharts.chart(div, {
        chart: {
            type: 'area'
        },
        title: {
            text: title //'CFD Traiket Semana 1-9'
        },
        subtitle: {
            text: subtitle//'Source: redmine.example.com'
        },
        xAxis: {
            //categories: ['1750', '1800', '1850', '1900', '1950', '1999', '2050'],
            categories: categories,//['1', '2', '3', '4', '5', '6', '7', '8', '9'],
            tickmarkPlacement: 'on',
            title: {
                enabled: false
            },
            labels: {
                formatter: function () {
                    return this.value; //dateFromDay(2019, this.value);
                }
            }
        },
        yAxis: {
            title: {
                text: yTitles //'Chamado'
            },
            labels: {
                formatter: function () {
                    return this.value;/// 1000;
                }
            }
        },
        tooltip: {
            split: true,
            valueSuffix: valueSuffix  // ' Chamado(s)'
        },
        plotOptions: {
            area: {
                stacking: 'normal',
                lineColor: '#666666',
                lineWidth: 1,
                marker: {
                    lineWidth: 1,
                    lineColor: '#666666'
                }
            }
        },
        series: series
        /*
        series: [{
            name: 'Asia',
            data: [502, 635, 809, 947, 1402, 3634, 5268]
        }, {
            name: 'Africa',
            data: [106, 107, 111, 133, 221, 767, 1766]
        }, {
            name: 'Europe',
            data: [163, 203, 276, 408, 547, 729, 628]
        }, {
            name: 'America',
            data: [18, 31, 54, 156, 339, 818, 1201]
        }, {
            name: 'Oceania',
            data: [2, 2, 2, 6, 13, 30, 46]
        }] */
    });
}