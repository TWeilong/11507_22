ymaps.ready(init);

function init() {
    const myMap = new ymaps.Map("map", {
        center: [55.835422, 49.160232],
        zoom: 18
    });

    const placemark = new ymaps.Placemark(
        [55.835422, 49.160232],
        {
            balloonContent: "Казань"
        },
        {
            preset: "islands#redIcon"
        }
    );

    myMap.geoObjects.add(placemark);
}