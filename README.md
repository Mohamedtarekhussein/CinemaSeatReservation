# CinemaSeatReservation
This project is a simple cinema seat reservation system built using F# and Windows Forms. The system allows users to view available seats, select seats to reserve, and enter customer information. It dynamically updates the UI to reflect seat availability and reservations.

![Project Screenshot](https://github.com/M-craspo/CinemaSeatReservation/blob/main/Cinema%20Seat%20Reservation%2012_2_2024%208_59_57%20PM.png)

## Features

- **Dynamic Cinema Hall Layout**:  
  Displays a grid of seats, where users can view available (green) or reserved (red) seats in real-time.

- **Seat Reservation**:  
  Users can select an available seat and reserve it by entering their customer details.

- **Ticket Generation**:  
  After booking a seat, a unique ticket is generated with the following details:  
  - **Ticket ID**  
  - **Customer Name**  
  - **Seat Information**  

- **User Interface**:  
  - Simple and intuitive interface.  
  - Customer name input and ticket list display.  
  - Easily navigate through the cinema hall to reserve your seat.

## Requirements

- **.NET SDK**:  
  The application uses the .NET framework. Make sure the .NET SDK is installed on your machine. You can download it from [here](https://dotnet.microsoft.com/download).

- **F#**:  
  F# compiler is required to run the project. You can install F# as part of the .NET SDK.

- **Windows Forms**:  
  Used for creating the graphical user interface (GUI). Ensure that your environment supports Windows Forms (typically included with Visual Studio or .NET SDK).

## How to Run

1. Clone this repository or download the project.

2. Install the **.NET SDK** (if not already installed). You can download it from [here](https://dotnet.microsoft.com/download).

3. Open a terminal or command prompt and navigate to the project directory.

4. Build and run the application:

   - If you are using Visual Studio or any other IDE that supports F#, open the solution and run the project.
   - Or, run the following commands from the terminal (in the project directory):

   ```bash
   dotnet build
   dotnet run
    ```
   5.The application window will appear with a cinema hall layout. You can now begin reserving seats.
   ## Usage

- **Customer Name**: Enter the customer name in the provided input field.
- **Book Seat**: Click on an available seat to reserve it. A ticket will be generated for that seat.
- **Movie Screen**: The movie screen is displayed below the seat grid.

## Code Overview

The code is divided into several sections:

### Domain Types:
- **SeatStatus**: Represents whether a seat is available or reserved.
- **Seat**: Contains the row, column, and status of a seat.
- **Ticket**: Holds ticket details such as ticket ID, seat, customer name, and showtime.
- **CinemaHall**: Represents the cinema hall with rows, columns, and a 2D array of seats.

### CinemaHallOperations:
- **createCinemaHall**: Initializes a cinema hall with a specified number of rows and columns, where all seats are available.
- **bookSeat**: Books a seat by changing its status to reserved and generates a ticket.

### UI (Windows Forms):
- **CinemaReservationForm**: The main form that contains the seat grid, customer name input, book seat button, and ticket list box. The seats are represented as buttons, and their color changes based on availability (green for available, red for reserved).
- **Ticket List**: Displays a list of booked tickets.

   
