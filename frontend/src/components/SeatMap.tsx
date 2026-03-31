import React, { useState } from 'react';
import { useGetShowSeatsQuery, useReserveSeatMutation } from '../store/api/apiSlice';
import { Armchair, CheckCircle } from 'lucide-react';

interface SeatMapProps {
  show: any;
}

const SeatMap: React.FC<SeatMapProps> = ({ show }) => {
  const { data: details, isLoading } = useGetShowSeatsQuery(show.id);
  const [reserveSeat, { isLoading: isBooking }] = useReserveSeatMutation();
  const [selectedSeat, setSelectedSeat] = useState<any>(null);
  const [message, setMessage] = useState('');

  if (isLoading) return <div className="loading">Mapping theatre seats...</div>;

  const handleReserve = async (seat: any) => {
    if (seat.status !== 'Available') return;
    
    try {
      const result = await reserveSeat({ showId: show.id, seatId: seat.id }).unwrap();
      if (result.success) {
        setSelectedSeat(seat);
        setMessage(result.message);
      } else {
        setMessage(result.message);
      }
    } catch (err: any) {
      setMessage(err.data?.message || 'Booking failed');
    }
  };

  return (
    <div className="seat-map-container">
      <div className="movie-info">
        <h2>{show.title}</h2>
        <p>Screen 1 | Status: {selectedSeat ? 'Seat Reserved' : 'Select a seat'}</p>
      </div>

      <div className="theatre-layout">
        <div className="screen">SCREEN</div>
        
        <div className="seat-grid">
          {details?.seats.map((seat: any) => (
            <div 
              key={seat.id} 
              className={`seat ${seat.status.toLowerCase()} ${selectedSeat?.id === seat.id ? 'selected' : ''}`}
              onClick={() => handleReserve(seat)}
              title={`${seat.seatNumber} - ${seat.tier} ($${seat.price})`}
            >
              <Armchair size={20} />
              <span className="tooltip">{seat.seatNumber}</span>
            </div>
          ))}
        </div>

        <div className="legend">
          <div className="legend-item"><div className="seat available"><Armchair size={14}/></div> Available</div>
          <div className="legend-item"><div className="seat booked"><Armchair size={14}/></div> Booked</div>
          <div className="legend-item"><div className="seat selected"><Armchair size={14}/></div> Selected</div>
        </div>
      </div>

      {message && (
        <div className="booking-status">
          <CheckCircle size={20} className="success-icon" />
          <span>{message}</span>
        </div>
      )}
    </div>
  );
};

export default SeatMap;
