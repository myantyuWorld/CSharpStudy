// subber.js
var zmq = require("zeromq"),
sock = zmq.socket("sub");

sock.connect("tcp://127.0.0.1:12000");
sock.subscribe("B");
console.log("Subscriber connected to port 12000");

sock.on("message", function(topic, message) {
console.log(
  "received a message related to:",
  topic,
  "containing message:",
  message
);
});