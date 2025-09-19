const http = require('http');
const sqlite3InitModule = require('./sqlite3.js');

http.createServer((req, res) => {
    res.setHeader('Access-Control-Allow-Origin', '*');
    res.setHeader('Access-Control-Allow-Methods', 'GET, POST, PUT, DELETE');
    res.setHeader('Access-Control-Allow-Headers', 'Content-Type');

    if (req.url === '/tasks' && req.method === 'GET') {
        res.writeHead(200, { 'Content-Type': 'application/json' });
        
        // TODO: switch this to load from the database
        tasks = [];
        for (let i = 1; i <= 100; i++) {
            tasks.push({ name: `task ${i}`, description: `do something ${i}` })
        }

        res.end(JSON.stringify(tasks));
    } else {
        res.writeHead(404);
        res.end();
    }
}).listen(3000, () => console.log('Server on port 3000'));