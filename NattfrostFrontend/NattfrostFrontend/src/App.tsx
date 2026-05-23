import { useState } from 'react'
import './App.css'
import logo from './assets/test_copy_2.png'

function App() {
  const [email, setEmail] = useState('')
  const [stad, setStad] = useState('')
  const [showModal, setShowModal] = useState(false)

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    try {
      const response = await fetch('http://localhost:5111/api/subscriber', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, city: stad }),
      })
      if (response.ok) {
        setEmail('')
        setStad('')
        setShowModal(true)
      } else {
        const text = await response.text()
        alert(text || 'Något gick fel. Försök igen.')
      }
    } catch {
      alert('Kunde inte nå servern. Kontrollera att backend körs.')
    }
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

      {showModal && (
        <div className="Modal-overlay" onClick={() => setShowModal(false)}>
          <div className="Modal" onClick={(e) => e.stopPropagation()}>
            <p className="Modal-text">
              Du är nu registrerad och kommer att få ett email om det finns risk för nattfrost i ditt område.
            </p>
            <button className="Modal-close" onClick={() => setShowModal(false)}>Ok</button>
          </div>
        </div>
      )}
    </div>
  )
}

export default App