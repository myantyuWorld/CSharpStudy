function getRandomInt(max) {
  return Math.floor(Math.random() * max);
}

// producer.js
let fs = require('fs');
var zmq = require("zeromq"),
  sock = zmq.socket("pub");

var zmq = require("zeromq"),
sock2 = zmq.socket("pub");

sock.bindSync("tcp://127.0.0.1:12345");
sock2.bindSync("tcp://127.0.0.1:12000");
console.log("Producer bound to port 12345");
console.log("Producer bound to port 12000");

var count = 0;
setInterval(function () {

  console.log("sending work");
  var obj = {
    CamNum: getRandomInt(4) + 1,
    Mbps: getRandomInt(10)
  }
  sock.send(["A", JSON.stringify(obj)]);
}, 500);

setInterval(function () {
  let base64_data = "";
    fs.readFile( 'image.jpg', function( err, content ) {
      if( err ) {
        console.error(err);
      }
      else {
        /* Base64変換 */
        base64_data = content.toString( 'base64' );
        console.log("sending label");
        var obj = {
          WorkLabel: "Hello:" + count++,
          Alert: count % 5 == 0 ? true : false,
          TimeStamp: Date.now().toString(),
          ImageStr: base64_data
        }
        sock2.send(["B", JSON.stringify(obj)]);
      }
    });
}, 100);