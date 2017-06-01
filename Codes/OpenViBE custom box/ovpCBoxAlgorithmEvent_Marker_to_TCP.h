#ifndef __OpenViBEPlugins_BoxAlgorithm_Event_Marker_to_TCP_H__
#define __OpenViBEPlugins_BoxAlgorithm_Event_Marker_to_TCP_H__

#ifdef TARGET_HAS_Boost
//You may have to change this path to match your folder organisation
#include "../ovp_defines.h"

#include <openvibe/ov_all.h>
#include <toolkit/ovtk_all.h>

#include <iostream>
#include <string>
#include <boost/bind.hpp>
#include <boost/asio.hpp>
// The unique identifiers for the box and its descriptor.
// Identifier are randomly chosen by the skeleton-generator.
#define OVP_ClassId_BoxAlgorithm_Event_Marker_to_TCP OpenViBE::CIdentifier(0x01395B83, 0x1EC44996)
#define OVP_ClassId_BoxAlgorithm_Event_Marker_to_TCPDesc OpenViBE::CIdentifier(0x6A8D65D0, 0x5245481F)

namespace OpenViBEPlugins
{
	namespace NetworkIO
	{
		/**
		 * \class CBoxAlgorithmEvent_Marker_to_TCP
		 * \author Neha Thomas (Drexel)
		 * \date Wed Apr 19 11:50:25 2017
		 * \brief The class CBoxAlgorithmEvent_Marker_to_TCP describes the box Event Marker to TCP.
		 *
		 */
		class CBoxAlgorithmEvent_Marker_to_TCP : virtual public OpenViBEToolkit::TBoxAlgorithm < OpenViBE::Plugins::IBoxAlgorithm >
		{
		public:
			virtual void release(void) { delete this; }

			virtual OpenViBE::boolean initialize(void);
			virtual OpenViBE::boolean uninitialize(void);
				
			//Here is the different process callbacks possible
			// - On clock ticks :
			//virtual OpenViBE::boolean processClock(OpenViBE::CMessageClock& rMessageClock);		
			// - On new input received (the most common behaviour for signal processing) :
			virtual OpenViBE::boolean processInput(OpenViBE::uint32 ui32InputIndex);
			// - On message received :
			virtual OpenViBE::boolean processMessage(const OpenViBE::Kernel::IMessageWithData& msg, OpenViBE::uint32 inputIndex);	
			
			// If you want to use processClock, you must provide the clock frequency.
			//virtual OpenViBE::uint64 getClockFrequency(void);
			
			virtual OpenViBE::boolean process(void);
			
			// As we do with any class in openvibe, we use the macro below 
			// to associate this box to an unique identifier. 
			// The inheritance information is also made available, 
			// as we provide the superclass OpenViBEToolkit::TBoxAlgorithm < OpenViBE::Plugins::IBoxAlgorithm >
			_IsDerivedFromClass_Final_(OpenViBEToolkit::TBoxAlgorithm < OpenViBE::Plugins::IBoxAlgorithm >, OVP_ClassId_BoxAlgorithm_Event_Marker_to_TCP);

		protected:
			// Input decoder:
			OpenViBEToolkit::TStimulationDecoder < CBoxAlgorithmEvent_Marker_to_TCP > m_oInputDecoder;
			//std::vector < OpenViBEToolkit::TStimulationDecoder < CBoxAlgorithmEvent_Marker_to_TCP >* > m_vStreamDecoder;
			// Output decoder:
			
		};


		// If you need to implement a box Listener, here is a sekeleton for you.
		// Use only the callbacks you need.
		// For example, if your box has a variable number of input, but all of them must be stimulation inputs.
		// The following listener callback will ensure that any newly added input is stimulations :	
		
		// The box listener can be used to call specific callbacks whenever the box structure changes : input added, name changed, etc.
		// Please uncomment below the callbacks you want to use.
		/*class CBoxAlgorithmEvent_Marker_to_TCPListener : public OpenViBEToolkit::TBoxListener < OpenViBE::Plugins::IBoxListener >
		{
		public:
			virtual OpenViBE::boolean onInputAdded(OpenViBE::Kernel::IBox& rBox, const OpenViBE::uint32 ui32Index)
		{
			rBox.setInputType(ui32Index, OV_TypeId_Stimulations);
			return true;
		};
			//virtual OpenViBE::boolean onInitialized(OpenViBE::Kernel::IBox& rBox) { return true; };
			//virtual OpenViBE::boolean onNameChanged(OpenViBE::Kernel::IBox& rBox) { return true; };
			//virtual OpenViBE::boolean onInputConnected(OpenViBE::Kernel::IBox& rBox, const OpenViBE::uint32 ui32Index) { return true; };
			//virtual OpenViBE::boolean onInputDisconnected(OpenViBE::Kernel::IBox& rBox, const OpenViBE::uint32 ui32Index) { return true; };
			//virtual OpenViBE::boolean onInputAdded(OpenViBE::Kernel::IBox& rBox, const OpenViBE::uint32 ui32Index) { return true; };
			//virtual OpenViBE::boolean onInputRemoved(OpenViBE::Kernel::IBox& rBox, const OpenViBE::uint32 ui32Index) { return true; };
			//virtual OpenViBE::boolean onInputTypeChanged(OpenViBE::Kernel::IBox& rBox, const OpenViBE::uint32 ui32Index) { return true; };
			//virtual OpenViBE::boolean onInputNameChanged(OpenViBE::Kernel::IBox& rBox, const OpenViBE::uint32 ui32Index) { return true; };
			//virtual OpenViBE::boolean onOutputConnected(OpenViBE::Kernel::IBox& rBox, const OpenViBE::uint32 ui32Index) { return true; };
			//virtual OpenViBE::boolean onOutputDisconnected(OpenViBE::Kernel::IBox& rBox, const OpenViBE::uint32 ui32Index) { return true; };
			//virtual OpenViBE::boolean onOutputAdded(OpenViBE::Kernel::IBox& rBox, const OpenViBE::uint32 ui32Index) { return true; };
			//virtual OpenViBE::boolean onOutputRemoved(OpenViBE::Kernel::IBox& rBox, const OpenViBE::uint32 ui32Index) { return true; };
			//virtual OpenViBE::boolean onOutputTypeChanged(OpenViBE::Kernel::IBox& rBox, const OpenViBE::uint32 ui32Index) { return true; };
			//virtual OpenViBE::boolean onOutputNameChanged(OpenViBE::Kernel::IBox& rBox, const OpenViBE::uint32 ui32Index) { return true; };
			//virtual OpenViBE::boolean onSettingAdded(OpenViBE::Kernel::IBox& rBox, const OpenViBE::uint32 ui32Index) { return true; };
			//virtual OpenViBE::boolean onSettingRemoved(OpenViBE::Kernel::IBox& rBox, const OpenViBE::uint32 ui32Index) { return true; };
			//virtual OpenViBE::boolean onSettingTypeChanged(OpenViBE::Kernel::IBox& rBox, const OpenViBE::uint32 ui32Index) { return true; };
			//virtual OpenViBE::boolean onSettingNameChanged(OpenViBE::Kernel::IBox& rBox, const OpenViBE::uint32 ui32Index) { return true; };
			//virtual OpenViBE::boolean onSettingDefaultValueChanged(OpenViBE::Kernel::IBox& rBox, const OpenViBE::uint32 ui32Index) { return true; };
			//virtual OpenViBE::boolean onSettingValueChanged(OpenViBE::Kernel::IBox& rBox, const OpenViBE::uint32 ui32Index) { return true; };

			_IsDerivedFromClass_Final_(OpenViBEToolkit::TBoxListener < OpenViBE::Plugins::IBoxListener >, OV_UndefinedIdentifier);
		};
		

		/**
		 * \class CBoxAlgorithmEvent_Marker_to_TCPDesc
		 * \author Neha Thomas (Drexel)
		 * \date Wed Apr 19 11:50:25 2017
		 * \brief Descriptor of the box Event Marker to TCP.
		 *
		 */
		class CBoxAlgorithmEvent_Marker_to_TCPDesc : virtual public OpenViBE::Plugins::IBoxAlgorithmDesc
		{
		public:

			virtual void release(void) { }

			virtual OpenViBE::CString getName(void) const                { return OpenViBE::CString("Event Marker to TCP"); }
			virtual OpenViBE::CString getAuthorName(void) const          { return OpenViBE::CString("Neha Thomas"); }
			virtual OpenViBE::CString getAuthorCompanyName(void) const   { return OpenViBE::CString("Drexel"); }
			virtual OpenViBE::CString getShortDescription(void) const    { return OpenViBE::CString("Sends event markers to NeuroHub Network module"); }
			virtual OpenViBE::CString getDetailedDescription(void) const { return OpenViBE::CString(""); }
			virtual OpenViBE::CString getCategory(void) const            { return OpenViBE::CString("Network IO"); }
			virtual OpenViBE::CString getVersion(void) const             { return OpenViBE::CString("1"); }
			virtual OpenViBE::CString getStockItemName(void) const       { return OpenViBE::CString("gtk-connect"); }

			virtual OpenViBE::CIdentifier getCreatedClass(void) const    { return OVP_ClassId_BoxAlgorithm_Event_Marker_to_TCP; }
			virtual OpenViBE::Plugins::IPluginObject* create(void)       { return new OpenViBEPlugins::NetworkIO::CBoxAlgorithmEvent_Marker_to_TCP; }
			
			
			/*virtual OpenViBE::Plugins::IBoxListener* createBoxListener(void) const               { return new CBoxAlgorithmEvent_Marker_to_TCPListener; }
			virtual void releaseBoxListener(OpenViBE::Plugins::IBoxListener* pBoxListener) const { delete pBoxListener; }*/
			
			virtual OpenViBE::boolean getBoxPrototype(
				OpenViBE::Kernel::IBoxProto& rBoxAlgorithmPrototype) const
			{
				rBoxAlgorithmPrototype.addInput("Event Markers",OV_TypeId_Stimulations);

				//rBoxAlgorithmPrototype.addFlag(OpenViBE::Kernel::BoxFlag_CanModifyInput);
				//rBoxAlgorithmPrototype.addFlag(OpenViBE::Kernel::BoxFlag_CanAddInput);
				//rBoxAlgorithmPrototype.addInputSupport(OV_TypeId_Stimulations);
				
				//No output specified.To add outputs use :
//rBoxAlgorithmPrototype.addOutput("Output Name",OV_TypeId_XXXX);

				//rBoxAlgorithmPrototype.addFlag(OpenViBE::Kernel::BoxFlag_CanModifyOutput);
				//rBoxAlgorithmPrototype.addFlag(OpenViBE::Kernel::BoxFlag_CanAddOutput);

				//No Message input specified.To add Message inputs use :
//rBoxAlgorithmPrototype.addMessageInput("Input Name");

				//rBoxAlgorithmPrototype.addFlag(OpenViBE::Kernel::BoxFlag_CanModifyMessageInput);
				//rBoxAlgorithmPrototype.addFlag(OpenViBE::Kernel::BoxFlag_CanAddMessageInput);

				//No Message output specified.To add Message outputs use :
//rBoxAlgorithmPrototype.addMessageOutput("Output Name");

				//rBoxAlgorithmPrototype.addFlag(OpenViBE::Kernel::BoxFlag_CanModifyMessageOutput);
				//rBoxAlgorithmPrototype.addFlag(OpenViBE::Kernel::BoxFlag_CanAddMessageOutput);

				//No setting specified.To add settings use :
//rBoxAlgorithmPrototype.addSetting("Setting Name",OV_TypeId_XXXX,"default value");

				//rBoxAlgorithmPrototype.addFlag(OpenViBE::Kernel::BoxFlag_CanModifySetting);
				//rBoxAlgorithmPrototype.addFlag(OpenViBE::Kernel::BoxFlag_CanAddSetting);
				
				rBoxAlgorithmPrototype.addFlag(OpenViBE::Kernel::BoxFlag_IsUnstable);
				
				return true;
			}
			_IsDerivedFromClass_Final_(OpenViBE::Plugins::IBoxAlgorithmDesc, OVP_ClassId_BoxAlgorithm_Event_Marker_to_TCPDesc);
		};
	};
};

#endif // __OpenViBEPlugins_BoxAlgorithm_TCPWriter_H__

#endif // TARGET_HAS_Boost