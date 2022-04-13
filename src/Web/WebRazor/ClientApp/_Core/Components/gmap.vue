<style></style>
<template>
    <div :id="mapName" style="height:100%;width:100%;" v-cloak>
    </div>
</template>
<script>
    export default {
        props: {
            mapName: String,
            stores: Array,
            setCenter: Function,
        },
        data() {
            return {
                storeMarkers: [],

                navigator: {},
                centerPosition: { lat: 0, lng: 0 },
                map: {},
                marker: {},
                infoWindow: {},
                geocode: {}
            };
        },
        mounted() {
            const vm = this;

            vm.navigator = navigator;
            vm.navigator.getUserMedia = navigator.getUserMedia || navigator.webkitGetUserMedia || navigator.mozGetUserMedia || navigator.msGetUserMedia || navigator.oGetUserMedia;

            var timerId = setInterval(() => {
                if (google && google.maps) {
                    vm.initMap();

                    vm.$bus.$on('event:items-updated', vm.placeMarkers);

                    vm.$bus.$emit('event:gmap-ready');

                    clearInterval(timerId);
                }
            }, 250);

            //document.addEventListener("DOMContentLoaded", vm.initMap);

            vm.$bus.$on('event:center-position', () => {
                debugger;
            });
        },
        updated() {
            const vm = this;

            if (vm.storeMarkers.length > 0)
                return;



            vm.stores.forEach(store => {

                const infoWindow = new google.maps.InfoWindow({
                    //content: `<div class='card p-3 bg-info'>the quick brown foxs the lazy dog.</div>`
                    content: `<div>
                                    <h6>${store.name}</h6>
                                    <div><span class='fas fa-fw fa-phone mr-1'></span>${store.phoneNumber}</div>
                                    <div><span class='fas fa-fw fa-clock mr-1'></span>8am - 6pm</div>
                                </div>`
                });
                const marker = new google.maps.Marker({
                    //draggable: true,
                    //animation: google.maps.Animation.BOUNCE,

                    position: {
                        lat: store.geoX,
                        lng: store.geoY,
                    },
                    map: vm.map,
                    //label: {
                    //    text: '\uf072',
                    //    fontFamily: 'Fontawesome',
                    //},
                    //title: store.name,
                    store: store,
                    infoWindow
                });
                //debugger;
                //vm.$emit('onGeolocation', vm.centerPosition);



                google.maps.event.addListener(marker, 'click', function (event) {
                    const store = this.store;

                    //vm.map.setCenter({
                    //    lat: store.geoX,
                    //    lng: store.geoY,
                    //});
                    vm.$emit('onStoreMarkerClicked', store);


                });

                marker.addListener('mouseover', function () {
                    var iw = this.infoWindow;

                    iw.open(vm.map, this);
                });

                marker.addListener('mouseout', function () {
                    var iw = this.infoWindow;

                    iw.close(vm.map, this);
                });


                vm.storeMarkers.push(marker);
            })
        },
        methods: {
            initMap() {
                const vm = this;

                /*
                Build list of map types.
                You can also use var mapTypeIds = ["roadmap", "satellite", "hybrid", "terrain", "OSM"]
                but static lists suck when Google updates the default list of map types.
                */
                var mapTypeIds = [];
                //for (var type in google.maps.MapTypeId) {
                //    mapTypeIds.push(google.maps.MapTypeId[type]);
                //}
                mapTypeIds.push("OSM");
                const lastZoom = Number.parseInt(localStorage.getItem('zoom')) || 15;
                vm.map = new google.maps.Map(document.getElementById(vm.mapName), {
                    center: vm.centerPosition,//{ lat: 13.948779, lng: 120.733035 }, //13.948779,120.733035
                    zoom: lastZoom,
                    mapTypeId: "OSM",
                    mapTypeControl: false,
                    streetViewControl: false,
                    mapTypeControlOptions: {
                        mapTypeIds: mapTypeIds
                    }
                });

                vm.map.mapTypes.set("OSM", new google.maps.ImageMapType({
                    getTileUrl: function (coord, zoom) {
                        // "Wrap" x (longitude) at 180th meridian properly
                        // NB: Don't touch coord.x: because coord param is by reference, and changing its x property breaks something in Google's lib
                        var tilesPerGlobe = 1 << zoom;
                        var x = coord.x % tilesPerGlobe;
                        if (x < 0) {
                            x = tilesPerGlobe + x;
                        }
                        // Wrap y (latitude) in a like manner if you want to enable vertical infinite scrolling

                        return "https://tile.openstreetmap.org/" + zoom + "/" + x + "/" + coord.y + ".png";
                    },
                    tileSize: new google.maps.Size(256, 256),
                    name: "OpenStreetMap",
                    maxZoom: 18
                }));

                vm.infoWindow = new google.maps.InfoWindow;
                vm.geocoder = new google.maps.Geocoder;

                if (vm.navigator.geolocation) {

                    google.maps.event.addListener(vm.map, 'zoom_changed', function (arg) {
                        localStorage.setItem('zoom', this.zoom);
                    });

                    vm.navigator.geolocation.getCurrentPosition(function (position) {
                        vm.centerPosition = {
                            lat: position.coords.latitude,
                            lng: position.coords.longitude
                        };
                        vm.map.setCenter(vm.centerPosition);

                        vm.marker = new google.maps.Marker({
                            //draggable: true,
                            //animation: google.maps.Animation.BOUNCE,

                            position: vm.centerPosition,
                            map: vm.map,
                            title: "Your Current Location",
                            label: {
                                text: 'You',
                                //fontFamily: 'Fontawesome',
                            },
                        });
                        vm.$emit('onGeolocation', vm.centerPosition);

                        google.maps.event.addListener(vm.marker, 'dragend', function (event) {

                            vm.centerPosition = this.getPosition();

                            vm.geocodeLatLng();

                        });
                        google.maps.event.addListener(vm.marker, 'click', function (event) {
                            //const store = this.store;

                            //vm.$emit('onStoreMarkerClicked', store);
                        });

                        //vm.refresh();
                    }, function () {
                        vm.handleLocationError(true, vm.infoWindow, vm.map.getCenter());
                        vm.refresh();
                    });
                } else {
                    // Browser doesn't support Geolocation
                    vm.handleLocationError(false, vm.infoWindow, vm.map.getCenter());
                    vm.refresh();
                }

                //refresh();
            },
            handleLocationError(browserHasGeolocation, infoWindow, pos) {
                const vm = this;

                vm.infoWindow.setPosition(pos);
                vm.infoWindow.setContent(browserHasGeolocation ?
                    'Error: The Geolocation service failed.' :
                    'Error: Your browser doesn\'t support geolocation.');
                vm.infoWindow.open(map);
            },
            geocodeLatLng() {
                const vm = this;
                //var input = document.getElementById('Input_Latitude').value + ',' + document.getElementById('Input_Longitude').value;
                //var latlngStr = input.split(',', 2);
                //var latlng = { lat: parseFloat(latlngStr[0]), lng: parseFloat(latlngStr[1]) };
                vm.geocoder.geocode({ 'location': vm.centerPosition }, function (results, status) {

                    if (status === 'OK') {
                        vm.$emit('onAddress', results[0]);
                        vm.$emit('onGeolocation', { lat: vm.centerPosition.lat(), lng: vm.centerPosition.lng() });

                    } else {
                        window.alert('Geocoder failed due to: ' + status);
                    }
                });
            },
            refresh() {
                const vm = this;

                var mapOptions = {
                    center: new google.maps.LatLng(vm.centerPosition.lat, vm.centerPosition.lng),
                    zoom: 15,
                    mapTypeId: google.maps.MapTypeId.TERRAIN
                };

                //vm.map = new google.maps.Map(document.getElementById(vm.mapName), mapOptions);
                //vm.map.setTilt(45);

                //putMarkerDevices(map, items);

            },
            getCurrentLocation() {
                const vm = this;

                vm.navigator.geolocation.getCurrentPosition(function (position) {
                    vm.centerPosition = {
                        lat: position.coords.latitude,
                        lng: position.coords.longitude
                    };

                    vm.map.setCenter(vm.centerPosition);

                    //var latlng = new google.maps.LatLng(40.748774, -73.985763);
                    vm.marker.setPosition(vm.centerPosition);

                    //vm.marker = new google.maps.Marker({
                    //    draggable: true,
                    //    position: vm.centerPosition,
                    //    map: vm.map,
                    //    title: "Your Current Location"
                    //});
                    //debugger;
                    //vm.map.refresh();
                });
            }
        }
    }
</script>