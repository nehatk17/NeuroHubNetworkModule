/*

compile with g++ udpgui.cpp -o udpgui -std=c++11  `pkg-config gtkmm-3.0 --cflags --libs` -pthread -lwiringPi


if necessary use -g after g++ in compilation and link to use with gdb.

to run:
./gui_ex
or gdb ./gui_ex

*/

#include <gtkmm.h>
#include <iostream>
#include <stdio.h>
#include <stdlib.h>
#include <cstdlib>
#include <iostream>
#include <chrono>
#include <cstring>
#include <string.h> // memset
#include <pthread.h>
#include <sys/socket.h>
#include <sys/types.h>
#include <arpa/inet.h>
#include <netinet/in.h>
#include <unistd.h>
#include <netdb.h>
#include <vector>
#include <string>
#include <fstream>
#include <algorithm>
#include <sys/time.h>
#include <sched.h>
#include <time.h>
#include <sys/mman.h>
#include <errno.h>
#include <wiringPi.h>
#include <wiringSerial.h>
using namespace std;


// Gtk widgets
Gtk::SpinButton *spinbutton1 = 0;
Gtk::ComboBox *combobox1 = 0;
Gtk::RadioButton *radiotcp, *radioudp = 0;
Gtk::Button *button_stop, *button_start = 0;


//variables for network connection
#define PORT "8888"
#define IP_ADDR "192.168.137.99"
#define MAXLEN 1
int BACKLOG =0;
int serfd;
static unsigned int cli_count = 0;
size_t size = sizeof(struct sockaddr_in);
struct sockaddr_in their_addr;
vector<sockaddr_in> udparray;
vector<int> tcparray;
bool ard = false;
bool wiringpisetup = false;
bool exitard = false;
int sock;
int baudrate; //9600 or 115200;
int baudint = 0;

pthread_t servbegin;
bool tcpbool;
bool looprun = true;
bool ardrun = true;
bool enterard=false;


//network functions functions
void send_message_all_udp(char *s, int mysock);
void send_message_all_tcp(char *s);
void send_message_udp(char *s, int uid, int port);
void send_message_tcp(char *s, int uid);
void send_client_udp(char *s, int mysock, int port);
void send_client_tcp(char *s, int sock);
void *from_ard_udp(void *);
void *from_ard_tcp(void *);
void *handle_conn_udp(void *);
void *handle_conn_tcp(void *);
void *begin_server(void *);    



/*send message to original sender*/
void send_client_udp( char *s, int mysock, int myport){
	int i;
	for(i=0;i<BACKLOG;i++){
	    
			if(htons(udparray[i].sin_port) == myport){
				sendto(mysock,s,1,0,(struct sockaddr*)&udparray[i], sizeof udparray[i]);
			}
		
	}
}

//send message to all clients except original sender
void send_message_udp(char *s, int mysock, int myport){
	int i;
	for(i=0;i<BACKLOG;i++){
	
			if(htons(udparray[i].sin_port) != myport){
				sendto(mysock,s,1,0,(struct sockaddr*)&udparray[i], sizeof udparray[i]);
			}
		
	}
}
 
/* Send message to all clients */
void send_message_all_udp(char *s, int mysock){
	int i;
	for(i=0;i<BACKLOG;i++){
		
			sendto(mysock,s,1,0,(struct sockaddr*)&udparray[i], sizeof udparray[i]);
		
	}
}

/* /////    TCP send functions //// */
/*send message to original sender*/
void send_client_tcp( char *s, int mysock){
	int i;
	for(i=0;i<BACKLOG;i++){
		if(tcparray[i]){
			if(tcparray[i] == mysock){
				send(tcparray[i], s, 1,0);
			}
		}
	}
}

//send message to all clients except original sender
void send_message_tcp(char *s, int mysock){
	int i;
	for(i=0;i<BACKLOG;i++){
		if(tcparray[i]){
			if(tcparray[i] != mysock){
				send(tcparray[i], s, 1,0);
			}
		}
	}
}
 
/* Send message to all clients */
void send_message_all_tcp(char *s){
	int i;
	for(i=0;i<BACKLOG;i++){
		if(tcparray[i]){
			send(tcparray[i], s, 1,0);
		}
	}
}
/*receive messages from Arduino*/
void *from_ard_udp(void *thissock)
{
  int mysock = *(int*)thissock;
  int counter = 1;
  int ardint;
  char ardchar;
  char *mesg;
  while (ardrun)
 
  {
    if (serialDataAvail (serfd) == -1)
    {
	    fprintf(stdout, "(Serial) No data able to be received: %s\n", strerror (errno));
	    exit(EXIT_FAILURE);
    } 
  
   ardint = serialGetchar(serfd) ;
    
    //cout << ardint << endl;
    if (ardint > 0)
    {
    //cout << ardint << endl;
  // usleep(400);
     ardchar = char(ardint);
     mesg = &ardchar;
     
    send_message_all_udp(mesg, mysock); 
     
   
    
    }
   
  }
  cout << "exit from_ard" << endl;
  exitard= true;
}

/* handle the connections from client */
void *handle_conn_udp(void *pnewsock)
{
  int mysock = *(int*)pnewsock;
 

   char client_msg[MAXLEN];
 

  int read_size;
  struct timeval tv;
  
  int clientint;
  int myport;
    
while(looprun){
       
 read_size = recvfrom(mysock, client_msg, 1, 0, (struct sockaddr*)&their_addr, &size);


     clientint = int(*client_msg);
     

      
      
     myport = htons(their_addr.sin_port);
    if (clientint >0)
    {
    //usleep(1000);
    
    
    send_client_udp(client_msg,mysock,myport); //was send_message
  
    serialPuts (serfd, &(*client_msg)) ;
    
    }
 
     
  }
  cout << "exit handle -conn " << endl;

 
}

/* TCP data handling */
void *from_ard_tcp(void *)
//void *from_ard(int sock)
{
;
/* Declare ourself as a real time task */



  int counter = 1;
  int ardint;
  char ardchar;
  char *mesg;
  while (ardrun)
 
  {
    if (serialDataAvail (serfd) == -1)
    {
	    fprintf(stdout, "(Serial) No data able to be received: %s\n", strerror (errno));
	    exit(EXIT_FAILURE);
    } 
    
     ardint = serialGetchar (serfd) ;

    if (ardint > 0)
    {
    //usleep(1000);
  //  cout << ardint << endl;
      ardchar = char(ardint);
      mesg = &ardchar;
      

send_message_all_tcp(mesg);

    
    }
   
  }
  cout << "exit from_ard" << endl;
  exitard=true;
}

/* handle the connections from client */
void *handle_conn_tcp(void *pnewsock)
{
  int mysock = *(int*)pnewsock;
 

   char client_msg[MAXLEN];
 
  int read_size;

  int clientint;
  int mesgcount = 0;
  int myport;
  
 
  while(looprun){   
         
  
     read_size = recv(mysock, client_msg, 1, 0);
  
 
  
     clientint = int(*client_msg);
   
     
     if (clientint > 0)
     {
    // usleep(1000);
    client_msg[read_size] = '\0';
   
     /*  cout << "length of client message: " << strlen(client_msg) << endl;
       cout << "# bytes is : " << read_size << endl;     */   
   send_client_tcp(client_msg,mysock); //was send_message
   
    
     serialPuts(serfd, &(*client_msg)) ;
 
    
   
    }

     
  }
  cout << "exit handle -conn " << endl;
  
 
 
}

void *begin_server(void *)
{
  //pthread_setcanceltype(PTHREAD_CANCEL_ASYNCHRONOUS, NULL);
  looprun = true;
  ardrun = true;
  BACKLOG = spinbutton1->get_value_as_int();
  baudint = combobox1->get_active_row_number();
  if (baudint == 0)
  {
     baudrate = 9600;
  }
  else
  {
    baudrate = 115200;
  }
  tcpbool = radiotcp->get_active();

    
    struct addrinfo hints, *res;
    int reuseaddr = 1; // True 
    
    // Get the address info 
    memset(&hints, 0, sizeof hints);
    hints.ai_family = AF_INET;
    if (tcpbool==1)
    {
    hints.ai_socktype = SOCK_STREAM;
    }
    else{
    
    hints.ai_socktype = SOCK_DGRAM;} //TCP = SOCK_STREAM, SOCK_DGRAM = UDP)
    if (getaddrinfo(IP_ADDR, PORT, &hints, &res) != 0) {
        perror("getaddrinfo");
        exit (EXIT_FAILURE);
        //return 1; 
    }

    // Create the socket 
    sock = socket(res->ai_family, res->ai_socktype, res->ai_protocol);
    if (sock == -1) {
        perror("socket");
        exit (EXIT_FAILURE);
       // return 1;
    }

    // Enable the socket to reuse the address 
    if (setsockopt(sock, SOL_SOCKET, SO_REUSEADDR, &reuseaddr, sizeof(int)) == -1) {
        perror("setsockopt");
        ::close(sock);
        exit (EXIT_FAILURE);
        //shutdown(sock,2);
       // return 1;
    }

    // Bind to the address 
    if (bind(sock, res->ai_addr, res->ai_addrlen) == -1) {
        perror("bind");
        ::close(sock);
        exit (EXIT_FAILURE);
        //shutdown(sock,2);
        //return 0;
    }

    freeaddrinfo(res);
    if (tcpbool ==1)
    {
    if (listen(sock, BACKLOG) == -1) {
        perror("listen");
        exit (EXIT_FAILURE);
      
    }
    }
      
      if( (serfd= serialOpen("/dev/ttyAMA0", baudrate))<0) //opens on-board serial port, baud rate 9600
      {
      fprintf(stderr, "unable to open serial device: %s\n", strerror(errno));
      exit(EXIT_FAILURE);
      }
  
    if (!wiringpisetup)
    { 
      if (wiringPiSetup() == -1)
      {
        fprintf (stdout, "Unable to start wiring Pi: %s\n", strerror (errno));
        exit(EXIT_FAILURE);
      }
      else
      {
        wiringpisetup=true;
      }
    }
    cout << "listening for connections" << endl;
    // Main loop - accepting initial connections from the # clients specified.
    
    // Main loop 
    bool running = true;
    // Initialize clients 
    while (running)
    {  
    
      char client_msg[MAXLEN];
      if (tcpbool ==1)
      {
      size_t size = sizeof(struct sockaddr_in);
      struct sockaddr_in their_addr;
      int clilen = sizeof(their_addr);
      int newsock = accept(sock, (struct sockaddr*)&their_addr, &size);
      if (newsock == -1) 
      {
        perror("accept");
        exit (EXIT_FAILURE);
       // return -1;
      }
     
      
      cli_count++;
      printf("Got a connection from %s on port %d\n", inet_ntoa(their_addr.sin_addr), htons(their_addr.sin_port));
      tcparray.push_back(newsock);
      if (cli_count == BACKLOG)
      {
         cout << "Max clients reached" << endl;
        running = false;
        break;
      }
    
      }
      else{
      int byte_count = recvfrom(sock, client_msg, 1, 0, (struct sockaddr*)&their_addr, &size);
    
      
      cli_count++;
      printf("Got a connection from %s on port %d\n", inet_ntoa(their_addr.sin_addr), htons(their_addr.sin_port));
      udparray.push_back(their_addr);
      if (cli_count == BACKLOG)
      {
         cout << "Max clients reached" << endl;
        running = false;
        break;
      }
    }
   
    }
    /* Send message to all clients that server is ready to accept data */  
    
    char r = (char)(cli_count);
    
     char *mesg = &r;
    
    if (tcpbool == 1)
    {
      send_message_all_tcp(mesg);
    }
    else{
    
    send_message_all_udp(mesg,sock);
   }

    pthread_t *ptr, from_ard_t;
    ptr =static_cast<pthread_t*>(malloc(sizeof(pthread_t)*(cli_count)));
   
    
    int i;
    if (tcpbool==1)
    {
      for (i=0;i<(BACKLOG);i++)
    {
      if (pthread_create(&ptr[i], NULL, handle_conn_tcp, (void *)&tcparray[i]) != 0)//was newsock
		  {
        fprintf(stderr, "Failed to create thread\n");
        exit (EXIT_FAILURE);
  		}
      
    }
    
 
      if (pthread_create(&from_ard_t, NULL, from_ard_tcp, NULL)!=0)
      {
      fprintf(stderr, "Failed to create thread\n");
      }
      enterard=true;
    }
    else{
     for (i=0;i<(BACKLOG);i++)
      {
        if (pthread_create(&ptr[i], NULL, handle_conn_udp, (void *)&sock) != 0)//was newsock
		    {
          fprintf(stderr, "Failed to create thread\n");
        exit (EXIT_FAILURE);
  		  }
      
      }
    

      if (pthread_create(&from_ard_t, NULL, from_ard_udp, (void *)&sock)!=0)
      {
      fprintf(stderr, "Failed to create thread\n");
      }
      enterard=true;
    }
      cout << "Created threads with arduino" << endl;
      pthread_join(from_ard_t, NULL);
      
      cout << "joined arduino thread" << endl; 
  
    
    
    for(i = 0; i < (BACKLOG); i++)
    {
       pthread_join(ptr[i], NULL);
    }
    cout << "joined send/recv threads" << endl; 
    
  
    
    close(sock);
    serialClose(serfd);
    udparray.clear();
    tcparray.clear();
    cli_count = 0;
    pthread_exit(NULL);
    button_start->set_sensitive(true);
    button_stop->set_sensitive(false);
    
   
}

//GUI functions
void buttonstart_clicked()
{

  
  if (pthread_create(&servbegin, NULL, begin_server, NULL)!=0)
      {
      fprintf(stderr, "Failed to create thread\n");
      }
  
  
  button_start->set_sensitive(false);
  button_stop->set_sensitive(true);

}

void buttonstop_clicked()
{
  cout << "End" << endl;
 looprun= false;
 ardrun = false;
  pthread_cancel(servbegin);
  pthread_join(servbegin, NULL);

  serialClose(serfd);
  ::close(sock);
 udparray.clear();
 tcparray.clear();
  cli_count = 0;
  
  
  button_stop->set_sensitive(false);
  if (enterard)
  {
      while (!exitard)
    {
    }
      button_start->set_sensitive(true);
  }
  else
  {
    button_start->set_sensitive(true);
  }
enterard = false;
  
exitard = false;
}

int main(int argc, char **argv)
{

  Glib::RefPtr<Gtk::Application> app = Gtk::Application::create(argc, argv, "org.gtkmm.example");
  
  //Load the GtkBuilder file and instantiate its widgets:
  Glib::RefPtr<Gtk::Builder> refBuilder = Gtk::Builder::create();
  try
  {
    refBuilder->add_from_file("NeuroHubGui3.glade");
  }
  catch(const Glib::FileError& ex)
  {
    std::cerr << "FileError: " << ex.what() << std::endl;
    return 1;
  }
  catch(const Glib::MarkupError& ex)
  {
    std::cerr << "MarkupError: " << ex.what() << std::endl;
    return 1;
  }
  catch(const Gtk::BuilderError& ex)
  {
    std::cerr << "BuilderError: " << ex.what() << std::endl;
    return 1;
  }

 

  Gtk::Window *window1 = 0;
  
  refBuilder->get_widget("window1", window1); 
  refBuilder->get_widget("button_stop", button_stop);
  refBuilder->get_widget("button_start", button_start);
  refBuilder->get_widget("spinbutton1", spinbutton1);
  refBuilder->get_widget("combobox1", combobox1);
  refBuilder->get_widget("radiotcp",radiotcp);
  refBuilder->get_widget("radioudp",radioudp);

  
  
  Gtk::RadioButton::Group group = radiotcp->get_group();
  
  
  Glib::RefPtr<Gtk::Adjustment> m_adjustment = Gtk::Adjustment::create(1.0, 1.0, 9.0, 1.0, 9.0, 0.0);
  spinbutton1->set_adjustment(m_adjustment);

  // connect more signals
  combobox1->set_active(0);
  button_start->signal_clicked().connect(sigc::ptr_fun(buttonstart_clicked));
  button_stop->signal_clicked().connect(sigc::ptr_fun(buttonstop_clicked));
 

  app->run(*window1);

  return 0;
}