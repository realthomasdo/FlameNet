import logo from './logo.svg';
import './App.css';
import Header from './components/Header';
import Table from './components/Table';


function App() {
  return (
    <div className="App">
      {/*component name*/}
      <Header />
      <body>
        <img src={logo} className="App-logo" alt="logo" />
        <p>
          <Table/>
        </p>
        <a
          className="App-link"
          href="https://reactjs.org"
          target="_blank"
          rel="noopener noreferrer"
        >
          Learn React
        </a>
        </body>
    </div>
  );
}

export default App;
