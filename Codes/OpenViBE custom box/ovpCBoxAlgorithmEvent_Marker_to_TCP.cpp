#ifdef TARGET_HAS_Boost

#include "ovpCBoxAlgorithmEvent_Marker_to_TCP.h"

#include <iostream>
#include <string>
#include <boost/bind.hpp>
#include <boost/asio.hpp>

using namespace OpenViBE;
using namespace OpenViBE::Kernel;
using namespace OpenViBE::Plugins;

using namespace OpenViBEPlugins;
using namespace OpenViBEPlugins::NetworkIO;

using boost::asio::ip::tcp;
using namespace std;
	string ipaddr = "192.168.137.99";
    unsigned short port = 8888;
    
boost::asio::io_service ios_service;
tcp::endpoint ep(boost::asio::ip::address::from_string(ipaddr), port);
tcp::socket mysock(ios_service,ep.protocol());

boolean CBoxAlgorithmEvent_Marker_to_TCP::initialize(void)
{
	
	m_oInputDecoder.initialize(*this, 0);
	
	mysock.connect(ep);
	/*tcp::endpoint ep(boost::asio::ip::address::from_string(ipaddr), port);
    boost::asio::io_service ios;
    tcp::socket socket(ios,ep.protocol());
	
	socket.connect(ep);*/


	//socket.connect(endpoint);
	/*int cliint = 1;
	char r = (char)(cliint);
    
    char *mesg = &r;*/
	
	
	// If you need to retrieve setting values, use the FSettingValueAutoCast function.
	// For example :
	// - CString setting at index 0 in the setting list :
	// CString l_sSettingValue = FSettingValueAutoCast(*this->getBoxAlgorithmContext(), 0);
	// - unsigned int64 setting at index 1 in the setting list :
	// uint64 l_ui64SettingValue = FSettingValueAutoCast(*this->getBoxAlgorithmContext(), 1);
	// - float64 setting at index 2 in the setting list :
	// float64 l_f64SettingValue = FSettingValueAutoCast(*this->getBoxAlgorithmContext(), 2);
	// ...

	return true;
}
/*******************************************************************************/

boolean CBoxAlgorithmEvent_Marker_to_TCP::uninitialize(void)
{
	mysock.close();
	//mysock.shutdown(boost::asio::socket_base::shutdown_both);
	ios_service.stop();
	
	m_oInputDecoder.uninitialize();
	
	
	return true;
}
/*******************************************************************************/

/*
boolean CBoxAlgorithmEvent_Marker_to_TCP::processClock(IMessageClock& rMessageClock)
{
	// some pre-processing code if needed...

	// ready to process !
	getBoxAlgorithmContext()->markAlgorithmAsReadyToProcess();

	return true;
}
*/
/*******************************************************************************/

/*
uint64 CBoxAlgorithmEvent_Marker_to_TCP::getClockFrequency(void)
{
	// Note that the time is coded on a 64 bits unsigned integer, fixed decimal point (32:32)
	return 0LL<<32; // the box clock frequency
}
*/
/*******************************************************************************/


boolean CBoxAlgorithmEvent_Marker_to_TCP::processInput(uint32 ui32InputIndex)
{
	// some pre-processing code if needed...

	// ready to process !
	getBoxAlgorithmContext()->markAlgorithmAsReadyToProcess();

	return true;
}

/*******************************************************************************/


boolean CBoxAlgorithmEvent_Marker_to_TCP::processMessage(const IMessageWithData& msg, uint32 inputIndex)
{
	//If you know what the message should contain, you can directly access the values by using 
	//getters of the message class with known keys. Otherwise, you can loop over the contents to discover the keys.
	
	//You can get the first CString key of the message by calling this function
	//const CString *l_sKey = msg.getFirstCStringToken();
	//You can then go through all the keys by calling
	// getNextCStringToken(previousKey)
	//The function will return NULL when no more keys are found
#if 0
	while(l_sKey!=NULL)
	{
		l_sKey = msg.getNextCStringToken(*l_sKey);
		//and access the content with
		CString* l_sContent;
		boolean ok = msg.getValueCString(l_sKey, &l_sContent);
		//if ok is false, the retrieval was not successful
		//the message will be deleted when the function goes out of scope, store the value if you wish to use it later
	}
	
	//Same thing for the other types
	const CString *l_sMatrixKey = msg.getFirstIMatrixToken();
	while(l_sMatrixKey!=NULL)
	{
		l_sMatrixKey = msg.getNextIMatrixToken(*l_sMatrixKey);
		//and access the content with
		IMatrix* l_oContent;
		boolean ok = msg.getValueIMatrix(l_sMatrixKey, &l_oContent);
		//if ok is false, the retrieval was not successful
		//the message will be deleted when the function goes out of scope, store the value if you wish to use it later
		//for matrices, the copy is done that way
		//CMatrix * l_oLocalMatrix = new CMatrix();
		//OpenViBEToolkit::Tools::Matrix::copy(*l_oLocalMatrix, *l_oContent);
	}
#endif
	
	// Remember to return false in case the message was unexpected (user has made a wrong connection)	
	return true;
}

/*******************************************************************************/

boolean CBoxAlgorithmEvent_Marker_to_TCP::process(void)
{
	
	// the static box context describes the box inputs, outputs, settings structures
	//IBox& l_rStaticBoxContext=this->getStaticBoxContext();
	// the dynamic box context describes the current state of the box inputs and outputs (i.e. the chunks)
	//IBoxIO& l_rDynamicBoxContext=this->getDynamicBoxContext();

	// here is some useful functions:
	// - To get input/output/setting count:
	//l_rStaticBoxContext.getInputCount();
	// l_rStaticBoxContext.getOutputCount();
	
	// - To get the number of chunks currently available on a particular input :
	//l_rDynamicBoxContext.getInputChunkCount(input_index)
	
	// - To send an output chunk :
	// l_rDynamicBoxContext.markOutputAsReadyToSend(output_index, chunk_start_time, chunk_end_time);
	
	
	// A typical process iteration may look like this.
	// This example only iterate over the first input of type Signal, and output a modified Signal.
	// thus, the box uses 1 decoder (m_oSignalDecoder) and 1 encoder (m_oSignalEncoder)
	
	IBoxIO& l_rDynamicBoxContext=this->getDynamicBoxContext();

	//iterate over all chunk on input 0
	
		for(uint32 j=0; j<l_rDynamicBoxContext.getInputChunkCount(0); j++)
		{
			
		// decode the chunk i
			m_oInputDecoder.decode(j);
		// the decoder may have decoded 3 different parts : the header, a buffer or the end of stream.
		/*if(m_oSignalDecoder.isHeaderReceived())
		{
			// Header received. This happens only once when pressing "play". For example with a StreamedMatrix input, you now know the dimension count, sizes, and labels of the matrix
			// ... maybe do some process ...
			
			// Pass the header to the next boxes, by encoding a header on the output 0:
			m_oSignalEncoder.encodeHeader(0);
			// send the output chunk containing the header. The dates are the same as the input chunk:
			l_rDynamicBoxContext.markOutputAsReadyToSend(0, l_rDynamicBoxContext.getInputChunkStartTime(0, i), l_rDynamicBoxContext.getInputChunkEndTime(0, i));
		}*/
			if(m_oInputDecoder.isBufferReceived())
			{
			// Buffer received. For example the signal values
			// Access to the buffer can be done thanks to :
			 IStimulationSet* l_pStimulations = m_oInputDecoder.getOutputStimulationSet();
				//const IStimulationSet* l_pStimulationSet = m_vStreamDecoder[i]->getOutputStimulationSet();
				for(uint32 k=0; k<l_pStimulations->getStimulationCount(); k++)
				{
				uint64 l_ui64StimulationCode = l_pStimulations->getStimulationIdentifier(k);
				int stimcodeint = int(l_ui64StimulationCode);
				if (stimcodeint > 33800){
					stimcodeint = stimcodeint - 33572;
				}
				if (stimcodeint>33200){
					stimcodeint = stimcodeint - 33179;
				}
				else if (stimcodeint>33000){
					stimcodeint = stimcodeint - 32964;
				}
				else if (stimcodeint >32700){
					stimcodeint = stimcodeint - 32734;
				}
				else if (stimcodeint > 1000){
					stimcodeint = stimcodeint - 805;
				}
				else if (stimcodeint > 890){
					stimcodeint = stimcodeint - 694;
				}
				else if (stimcodeint > 760){
					stimcodeint = stimcodeint - 598;
				}
				else if (stimcodeint > 250){
					stimcodeint = stimcodeint - 147;
				}
				char s = (char)(stimcodeint);
				char *mesg = &s;
				mysock.write_some(boost::asio::buffer(mesg, 1));
				}
			}
			
			
		/*if(m_oSignalDecoder.isEndReceived())
		{
			// End of stream received. This happens only once when pressing "stop". Just pass it to the next boxes so they receive the message :
			m_oSignalEncoder.encodeEnd(0);
			l_rDynamicBoxContext.markOutputAsReadyToSend(0, l_rDynamicBoxContext.getInputChunkStartTime(0, i), l_rDynamicBoxContext.getInputChunkEndTime(0, i));
		}*/
		}
		// The current input chunk has been processed, and automaticcaly discarded.
		// you don't need to call "l_rDynamicBoxContext.markInputAsDeprecated(0, i);"
	
	

	// check the official developer documentation webpage for more example and information :
	
	// Tutorials:
	// http://openvibe.inria.fr/documentation/#Developer+Documentation
	// Codec Toolkit page :
	// http://openvibe.inria.fr/codec-toolkit-references/
	
	// Feel free to ask experienced developers on the forum (http://openvibe.inria.fr/forum) and IRC (#openvibe on irc.freenode.net).

	return true;
}
#endif // TARGET_HAS_Boost