import React, { useState } from 'react';
import { useGetShowsQuery } from '../store/api/apiSlice';
import { Calendar, Clock, LogOut, ChevronRight } from 'lucide-react';
import { useDispatch, useSelector } from 'react-redux';
import { logout } from '../store/slices/authSlice';
import { RootState } from '../store/store';
import SeatMap from './SeatMap';

const Dashboard: React.FC = () => {
  const { data: shows, isLoading, error } = useGetShowsQuery();
  const { user } = useSelector((state: RootState) => state.auth);
  const dispatch = useDispatch();
  const [selectedShow, setSelectedShow] = useState<any>(null);

  const handleLogout = () => {
    dispatch(logout());
  };

  if (isLoading) return <div className="loading">Loading cinematic experiences...</div>;
  if (error) return <div className="error">Failed to load shows. Please try again.</div>;

  return (
    <div className="dashboard">
      <nav className="navbar">
        <div className="logo">CineSynk</div>
        <div className="user-menu">
          <span className="welcome">Hi, {user}</span>
          <button onClick={handleLogout} className="logout-btn">
            <LogOut size={18} />
          </button>
        </div>
      </nav>

      {selectedShow ? (
        <div className="booking-session">
          <div className="header-actions">
            <button onClick={() => setSelectedShow(null)} className="back-btn">
              Back to Shows
            </button>
          </div>
          <SeatMap show={selectedShow} />
        </div>
      ) : (
        <section className="shows-section">
          <h2>Now Showing</h2>
          <div className="shows-grid">
            {shows?.map((show) => (
              <div key={show.id} className="show-card">
                <div className="card-media">
                  {/* In a real app, show.posterUrl would be here */}
                  <div className="poster-placeholder">
                    <span>{show.title[0]}</span>
                  </div>
                </div>
                <div className="card-content">
                  <h3>{show.title}</h3>
                  <div className="show-meta">
                    <span className="meta-item">
                      <Calendar size={14} />
                      {new Date(show.startTime).toLocaleDateString()}
                    </span>
                    <span className="meta-item">
                      <Clock size={14} />
                      {new Date(show.startTime).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}
                    </span>
                  </div>
                  <button
                    className="book-now-btn"
                    onClick={() => setSelectedShow(show)}
                  >
                    Book Seats <ChevronRight size={16} />
                  </button>
                </div>
              </div>
            ))}
          </div>
        </section>
      )}
    </div>
  );
};

export default Dashboard;
