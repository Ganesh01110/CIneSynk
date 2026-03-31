import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useDispatch } from 'react-redux';
import { useLoginMutation } from '../store/api/apiSlice';
import { setCredentials } from '../store/slices/authSlice';
import { LogIn, UserPlus } from 'lucide-react';

const Login: React.FC = () => {
  const [phoneNumber, setPhoneNumber] = useState('');
  const [password, setPassword] = useState('');
  const [isRegistering, setIsRegistering] = useState(false);
  const [error, setError] = useState('');

  const [login, { isLoading }] = useLoginMutation();
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');

    try {
      const result = await login({ phoneNumber, password }).unwrap();
      if (result.success) {
        dispatch(setCredentials({ user: result.name, token: result.token }));
        navigate('/dashboard');
      } else {
        setError(result.message || 'Login failed');
      }
    } catch (err: any) {
      setError(err.data?.message || 'Something went wrong');
    }
  };

  return (
    <div className="login-container">
      <div className="login-card">
        <div className="icon-wrapper">
          <LogIn size={48} className="theme-icon" />
        </div>
        <h2>{isRegistering ? 'Create Account' : 'Welcome Back'}</h2>
        <p className="subtitle">
          {isRegistering 
            ? 'Join PulseQueue for seamless bookings' 
            : 'Login to manage your tickets'}
        </p>

        <form onSubmit={handleSubmit}>
          <div className="input-group">
            <label>Phone Number</label>
            <input 
              type="text" 
              placeholder="e.g. 1234567890" 
              value={phoneNumber}
              onChange={(e) => setPhoneNumber(e.target.value)}
              required
            />
          </div>
          <div className="input-group">
            <label>Password</label>
            <input 
              type="password" 
              placeholder="••••••••" 
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required 
            />
          </div>

          {error && <div className="error-message">{error}</div>}

          <button type="submit" className="login-btn" disabled={isLoading}>
            {isLoading ? 'Processing...' : (isRegistering ? 'Register' : 'Login')}
          </button>
        </form>

        <div className="toggle-mode">
          {isRegistering ? 'Already have an account?' : 'New to PulseQueue?'}
          <button 
            type="button" 
            onClick={() => setIsRegistering(!isRegistering)}
            className="text-btn"
          >
            {isRegistering ? 'Login Instead' : 'Create Account'}
          </button>
        </div>
      </div>
    </div>
  );
};

export default Login;
