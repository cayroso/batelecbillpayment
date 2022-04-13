<style></style>
<template>
    <div :id="mapName" style="height:100%;width:100%;" v-cloak></div>
</template>
<script>
    export default {
        props: {
            mapName: String,
            draggable: Boolean,
            fixed: Boolean,
            showLocation: Boolean,
            cx: Number,
            cy: Number,
            markerClickAction: Function,
            //items: Array,
        },
        watch: {
            'cx': function (newValue, oldValue) {
                const vm = this;
                //debugger
                if (vm.map.setCenter) {
                    vm.centerPosition.lat = newValue;
                    vm.map.setCenter(vm.centerPosition);
                    vm.setMarker();
                }
            },
            'cy': function (newValue, oldValue) {
                const vm = this;

                if (vm.map.setCenter) {
                    vm.centerPosition.lng = newValue;
                    vm.map.setCenter(vm.centerPosition);
                    vm.setMarker();
                }
            }
        },
        data() {
            return {
                navigator: {},
                centerPosition: { lat: 0, lng: 0 },// { lat: 13.8954684059025, lng: 120.906667412659 },
                map: {},
                marker: {},
                infoWindow: {},
                geocode: {},
                items: [],
                markers: [],
            };
        },
        created() {
            const vm = this;
        },
        beforeUpdate() {
            debugger;
        },
        updated() {
            debugger;
        },
        async mounted() {
            const vm = this;

            vm.navigator = navigator;
            vm.navigator.getUserMedia = navigator.getUserMedia || navigator.webkitGetUserMedia || navigator.mozGetUserMedia || navigator.msGetUserMedia || navigator.oGetUserMedia;

            var timerId = setInterval(_ => {
                if (google && google.maps) {

                    clearInterval(timerId);

                    vm.initMap();

                    vm.$emit('onMapReady');
                }
            }, 250);
        },

        methods: {
            async initMap() {
                const vm = this;

                /*
                Build list of map types.
                You can also use var mapTypeIds = ["roadmap", "satellite", "hybrid", "terrain", "OSM"]
                but static lists suck when Google updates the default list of map types.
                */
                var mapTypeIds = [];
                for (var type in google.maps.MapTypeId) {
                    mapTypeIds.push(google.maps.MapTypeId[type]);
                }
                mapTypeIds.push("OSM");

                const lastZoom = Number.parseInt(localStorage.getItem('zoom')) || 15;

                vm.map = new google.maps.Map(document.getElementById(vm.mapName), {
                    center: vm.centerPosition,//{ lat: 13.948779, lng: 120.733035 }, //13.948779,120.733035
                    zoom: lastZoom,
                    //mapTypeId: "OSM",
                    mapTypeControl: true,
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
                    name: "Open Street Map",
                    maxZoom: 18
                }));

                vm.infoWindow = new google.maps.InfoWindow;
                vm.geocoder = new google.maps.Geocoder;

                if (vm.navigator.geolocation) {

                    if (vm.cx === 0 && vm.cy === 0) {
                        await vm.getCurrentLocation();
                    }

                    google.maps.event.addListener(vm.map, 'zoom_changed', function (arg) {
                        localStorage.setItem('zoom', this.zoom);
                    });

                } else {
                    // Browser doesn't support Geolocation
                    vm.handleLocationError(false, vm.infoWindow, vm.map.getCenter());
                    debugger;
                }
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

                vm.geocoder.geocode({ 'location': vm.centerPosition }, function (results, status) {

                    if (status === 'OK') {
                        vm.$emit('onAddress', results[0], { lat: vm.centerPosition.lat, lng: vm.centerPosition.lng });
                        //vm.$emit('onGeolocation', { lat: vm.centerPosition.lat(), lng: vm.centerPosition.lng() });

                    } else {
                        window.alert('Geocoder failed due to: ' + status);
                    }
                });
            },

            async getCurrentLocation() {
                const vm = this;

                await vm.navigator.geolocation.getCurrentPosition(function (position) {

                    //if (!vm.fixed) {
                    vm.centerPosition = {
                        lat: position.coords.latitude,
                        lng: position.coords.longitude
                    };

                    //}

                    vm.map.setCenter(vm.centerPosition);

                    if (vm.showLocation) {

                        vm.setMarker();
                        //vm.marker = new google.maps.Marker({
                        //    draggable: vm.draggable,
                        //    //animation: google.maps.Animation.BOUNCE,

                        //    position: vm.centerPosition,
                        //    map: vm.map,
                        //    //title: "Your Current Location",
                        //    //label: {
                        //    //    text: 'You',
                        //    //    //fontFamily: 'Fontawesome',
                        //    //},
                        //});

                        ////var latlng = new google.maps.LatLng(40.748774, -73.985763);
                        //vm.marker.setPosition(vm.centerPosition);

                        //google.maps.event.addListener(vm.marker, 'dragend', function (event) {
                        //    vm.centerPosition = this.getPosition();
                        //    vm.geocodeLatLng();
                        //});
                    }

                });

            },

            setMarker() {
                const vm = this;

                if (vm.marker && vm.marker.setMap)
                    vm.marker.setMap(null);

                vm.marker = new google.maps.Marker({
                    draggable: vm.draggable,
                    //animation: google.maps.Animation.BOUNCE,

                    position: vm.centerPosition,
                    map: vm.map,
                    //title: "Your Current Location",
                    //label: {
                    //    text: 'You',
                    //    //fontFamily: 'Fontawesome',
                    //},
                });

                //var latlng = new google.maps.LatLng(40.748774, -73.985763);
                vm.marker.setPosition(vm.centerPosition);

                google.maps.event.addListener(vm.marker, 'dragend', function (event) {
                    //vm.centerPosition = this.getPosition();
                    const pos = this.getPosition();
                    
                    vm.centerPosition.lat = pos.lat();
                    vm.centerPosition.lng = pos.lng();
                    vm.geocodeLatLng();
                    
                });
            },

            placeMarkers(items, recenter) {
                const vm = this;
                let markers = vm.markers;

                if (markers && markers.length > 0) {
                    markers.forEach(marker => marker.setMap(null));
                }

                if (items && items.length) {

                    markers = [];

                    items.forEach(item => {
                        var marker = new google.maps.Marker({
                            draggable: false,
                            //animation: google.maps.Animation.BOUNCE,

                            position: {
                                lat: item.geoX,
                                lng: item.geoY,
                            },
                            map: vm.map,
                            item: item,
                            title: `${item.firstName} ${item.lastName}`,
                            //label: {
                            //    text: 'You',
                            //    //fontFamily: 'Fontawesome',
                            //},
                        });

                        google.maps.event.addListener(marker, 'click', function (event) {

                            if (vm.markerClickAction)
                                vm.markerClickAction(this.item);
                        });

                        markers.push(marker);
                    });

                    vm.markers = markers;
                }


                //if (recenter) {
                vm.map.setCenter(vm.centerPosition);
                //}
            }
        }
    }
</script>