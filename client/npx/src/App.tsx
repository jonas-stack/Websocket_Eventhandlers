import { useState, useEffect } from 'react';

function App() {
    const [socket, setSocket] = useState<WebSocket | null>(null);
    const [messages, setMessages] = useState<string[]>([]);
    const [input, setInput] = useState<string>('');

    useEffect(() => {
        const ws = new WebSocket('ws://localhost:8080');
        ws.onopen = () => console.log('Connected to WebSocket');
        ws.onmessage = (event) => {
            const data = JSON.parse(event.data);
            if (data.type === 'ServerSendsMessageToRoom') {
                setMessages((prevMessages) => [...prevMessages, data.text]);
            }
        };
        ws.onclose = () => console.log('Disconnected from WebSocket');
        setSocket(ws);

        return () => {
            ws.close();
        };
    }, []);

    const sendMessage = () => {
        if (socket) {
            const message = {
                type: 'ClientWantsToSendMessageToRoom',
                jwt: 'your-jwt-token',
                text: input,
                requestId: 'unique-request-id',
                sender: 'your-username',
            };
            socket.send(JSON.stringify(message));
            setInput('');
        }
    };

    return (
        <div>
            <h1>WebSocket Chat</h1>
            <div>
                {messages.map((msg, index) => (
                    <div key={index}>{msg}</div>
                ))}
            </div>
            <input
                type="text"
                value={input}
                onChange={(e) => setInput(e.target.value)}
            />
            <button onClick={sendMessage}>Send</button>
        </div>
    );
}

export default App;