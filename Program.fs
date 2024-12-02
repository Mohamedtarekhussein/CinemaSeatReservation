open System
open System.Windows.Forms
open System.Drawing

// Immutable domain types
type SeatStatus = 
    | Available
    | Reserved

type Seat = {
    Row: int
    Column: int
    Status: SeatStatus
}

type Ticket = {
    TicketId: Guid
    Seat: Seat
    CustomerName: string
    Showtime: DateTime
}

// Cinema Hall as an immutable record
type CinemaHall = {
    Rows: int
    Columns: int
    Seats: Seat[,]
}

module CinemaHallOperations =
    // Pure function to create initial cinema hall
    let createCinemaHall rows columns : CinemaHall =
        let seats = 
            Array2D.init rows columns (fun row col -> 
                { Row = row; Column = col; Status = Available }
            )
        { 
            Rows = rows
            Columns = columns
            Seats = seats 
        }

    // Pure function to book a seat
    let bookSeat (hall: CinemaHall) (row: int) (col: int) (customerName: string) (showtime: DateTime) =
        // Check seat availability using pattern matching
        match hall.Seats.[row, col].Status with
        | Available ->
            // Create a new hall with updated seat status
            let updatedSeats = Array2D.copy hall.Seats
            updatedSeats.[row, col] <- { updatedSeats.[row, col] with Status = Reserved }
            
            // Create ticket
            let ticket = {
                TicketId = Guid.NewGuid()
                Seat = { Row = row; Column = col; Status = Reserved }
                CustomerName = customerName
                Showtime = showtime
            }
            
            // Return updated hall and ticket
            Some({ hall with Seats = updatedSeats }, ticket)
        | Reserved -> None

// Main Form for Seat Reservation
type CinemaReservationForm() as this =
    inherit Form()
    // Configurable hall dimensions
    let rows = 10
    let columns = 10
    // Mutable state for tracking hall and booked tickets
    let mutable cinemaHall = CinemaHallOperations.createCinemaHall rows columns
    let mutable bookedTickets : Ticket list = []
    // UI Components
    let seatPanel = new Panel()
    let customerNameInput = new TextBox()
    let bookButton = new Button()
    let ticketListBox = new ListBox()
    // Create seat buttons dynamically
    let createSeatButtons() =
        seatPanel.SuspendLayout()
        // Clear existing buttons
        seatPanel.Controls.Clear()
        // Create buttons for each seat
        for row in 0 .. rows - 1 do
            for col in 0 .. columns - 1 do
                let seatButton = new Button()
                seatButton.Size <- Size(40, 40)
                seatButton.Location <- Point(col * 45, row * 45)
                // Use pattern matching to set button color
                match cinemaHall.Seats.[row, col].Status with
                | Available -> seatButton.BackColor <- Color.Green
                | Reserved -> seatButton.BackColor <- Color.Red
                // Add click event
                seatButton.Click.Add(fun _ -> 
                    customerNameInput.Focus() |> ignore
                    bookButton.Tag <- (row, col)
                )
                seatPanel.Controls.Add(seatButton)
        seatPanel.ResumeLayout()

    // Book seat button click handler
    let bookSeatHandler (e: EventArgs) =
        try 
            let customerName = customerNameInput.Text.Trim()
            // Validate input
            if String.IsNullOrEmpty(customerName) then
                MessageBox.Show("Please enter customer name.") |> ignore
                customerNameInput.Focus() |> ignore
            else
                // Get selected seat coordinates
                match bookButton.Tag with
                | :? (int * int) as (row, col) ->
                    // Use functional approach to book seat
                    match CinemaHallOperations.bookSeat cinemaHall row col customerName DateTime.Now with
                    | Some(updatedHall, ticket) ->
                        // Update hall state
                        cinemaHall <- updatedHall
                        // Add ticket to list
                        bookedTickets <- ticket :: bookedTickets
                        // Refresh UI
                        createSeatButtons()
                        ticketListBox.Items.Add(
                            sprintf "Ticket %A: %s - Seat %d,%d" 
                                ticket.TicketId ticket.CustomerName ticket.Seat.Row ticket.Seat.Column
                        ) |> ignore
                        // Clear input
                        customerNameInput.Clear()
                    | None ->
                        MessageBox.Show("Seat is already reserved!") |> ignore
                | _ -> 
                    MessageBox.Show("Please select a seat first!") |> ignore
        with 
        | ex -> MessageBox.Show(ex.Message) |> ignore

    do
        // Form setup
        this.Text <- "Cinema Seat Reservation"
        this.Size <- Size(600, 500)
        // Seat Panel
        seatPanel.Location <- Point(20, 20)
        seatPanel.Size <- Size(450, 450)
        seatPanel.BorderStyle <- BorderStyle.FixedSingle
        this.Controls.Add(seatPanel)
        // Customer Name Input
        customerNameInput.Location <- Point(500, 50)
        customerNameInput.Size <- Size(150, 25)
        customerNameInput.PlaceholderText <- "Customer Name"
        this.Controls.Add(customerNameInput)
        // Book Button
        bookButton.Text <- "Book Seat"
        bookButton.Location <- Point(500, 100)
        bookButton.Size <- Size(100, 30)
        bookButton.Click.Add(bookSeatHandler)
        this.Controls.Add(bookButton)
        // Ticket List
        ticketListBox.Location <- Point(500, 200)
        ticketListBox.Size <- Size(150, 200)
        this.Controls.Add(ticketListBox)
        // Initial seat button creation
        createSeatButtons()

[<STAThread>]
do Application.Run(new CinemaReservationForm())