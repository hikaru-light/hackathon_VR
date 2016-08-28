var express = require('express');
var app = express();

var port = process.env.PORT || 3001;

//サーバーの立ち上げ
var http = require('http');

//指定したポートにきたリクエストを受け取れるようにする
var server = http.createServer(app).listen(port, function () {
  console.log('Server listening at port %d', port);
});

var WebSocketServer = require('ws').Server;
var wss = new WebSocketServer({server:server});

var uuid = require('uuid');

app.get('/user_token', function(req, res) {
  res.send(uuid.v4());
});

app.get('/', function(req, res){
  res.sendFile(__dirname + '/index.html');
});

var connections = []; 
wss.on('connection', function (ws) {
  console.log('connect!!');
  connections.push(ws);
  ws.on('close', function () {
    console.log('close');
    connections = connections.filter(function (conn, i) {
      return (conn === ws) ? false : true;
    });
  });
  ws.on('message', function (message) {
    console.log('message:', message);
    connections.forEach(function (con, i) {
      // Broadcast通信にしておく
      if(con !== ws){
        con.send(message);
      }
    });
  });
});