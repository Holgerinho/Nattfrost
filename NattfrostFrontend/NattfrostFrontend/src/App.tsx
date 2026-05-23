import { useState } from 'react'
import './App.css'
import logo from './assets/test_copy_2.png'

function App() {
  const [email, setEmail] = useState('')
  const [stad, setStad] = useState('')

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault()
    console.log({ email, stad })
  }

  return (
    <div className="App">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="Nattfrost" />
      </header>
      
      <hr className="Divider" />

      <main className="Content">
        <p className="Description">
          Nattfrost är en based app som varnar när det är risk för frost under natten.
          För att få en varning, ange din e-postadress och stad nedan:
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