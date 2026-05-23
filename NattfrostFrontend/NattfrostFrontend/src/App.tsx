import { useState } from 'react'
import logo from './assets/test copy 2.png'
import './App.css'

function App() {
  const [email, setEmail] = useState('')
  const [stad, setStad] = useState('')

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault()
    // TODO: submit to backend
    console.log({ email, stad })
  }

  return (
    <div className="page">
      <header className="site-header">
        <img src={logo} alt="Nattfrost" className="site-logo"/>
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

        <form className="signup-form" onSubmit={handleSubmit}>
          <label className="field-label" htmlFor="email">Email</label>
          <input
            id="email"
            type="email"
            className="field-input"
            value={email}
            onChange={e => setEmail(e.target.value)}
            required
          />

          <label className="field-label" htmlFor="stad">Stad</label>
          <input
            id="stad"
            type="text"
            className="field-input"
            value={stad}
            onChange={e => setStad(e.target.value)}
            required
          />

          <button type="submit" className="ok-button">Ok</button>
        </form>
      </main>
    </div>
  )
}

export default App
