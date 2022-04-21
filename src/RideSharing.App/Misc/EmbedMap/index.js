var directionsService;
var directionsRenderer;
var map;


function initMap() {
    directionsService = new google.maps.DirectionsService();
    directionsRenderer = new google.maps.DirectionsRenderer();
    const map = new google.maps.Map(document.getElementById("map"), {
        mapId: "147ee6441d1c655d",
        zoom: 6,
        center: { lat: 49.82, lng: 15.47 },
        disableDefaultUI: true
    });

    directionsRenderer.setMap(map);
}

function setRoute(from, to) {

    directionsService.route({
        origin: {query: from},
        destination: {query: to},
        travelMode: google.maps.TravelMode.DRIVING
    }).then((result) => {
        directionsRenderer.setDirections(result);
    }).catch((e) => {
        window.alert("Failed to display map.\n" + e);
    });
}

window.initMap = initMap;