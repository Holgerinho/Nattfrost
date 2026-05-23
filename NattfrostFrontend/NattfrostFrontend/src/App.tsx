import { useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from './assets/vite.svg'
import heroImg from './assets/hero.png'
import './App.css'
import logo from './assets/test_copy_2.png'

function App() {
  const [email, setEmail] = useState('')
  const [stad, setStad] = useState('')

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault()
    // TODO: submit to backend
    console.log({ email, stad })
  }

  return (
    <div className="App">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="Nattfrost" />
      </header>

      <hr className="divider" />

      <main className="content">
        <p className="description">
          Nattfrost är en based app som låter dig kolla om det blir frost
          under natten n-shit. Så att dina växter slipper bli rippedy rekt.
        </p>
        <p className="description">
          Fyll i email + stad i fälten nedan och tryck Ok. Vi skickar ett email runt kl
          19:00 varje dag om det blir kallt.
        </p>

        <form className="Form" onSubmit={handleSubmit}>
          {/* Email-blocket */}
          <div className="Form-group">
            <label className="Email-label" htmlFor="email">Email</label>
            <input
              id="email"
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              className="Email-input"
              required
            />  
          </div>

          {/* Stad-blocket */}
          <div className="Form-group">
            <label className="Stad-label" htmlFor="stad">Stad</label>
            <input
              id="stad"
              type="text"
              value={stad}
              onChange={(e) => setStad(e.target.value)}
              className="Stad-input"
              required
            />  
          </div>

          <button type="submit" className="Submit-button">Ok</button>
        </form>
      </main>
    </div>
  )
}

export default App