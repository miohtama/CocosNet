
EXTRA_DIST =  Makefile

# Warning: This is an automatically generated file, do not edit!

srcdir=.
top_srcdir=..

include $(top_srcdir)/config.make

ifeq ($(CONFIG),DEBUG_IPHONESIMULATOR)
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG"
ASSEMBLY = bin/Debug/CocosNetMac.exe
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = exe
PROJECT_REFERENCES = 
BUILD_DIR = bin/Debug

COCOSNETMAC_EXE_MDB_SOURCE=bin/Debug/CocosNetMac.exe.mdb
COCOSNETMAC_EXE_MDB=$(BUILD_DIR)/CocosNetMac.exe.mdb
MONOMAC_DLL_SOURCE=../../../../.config/MonoDevelop/addins/MonoDevelop.MonoMac.2.4.0.12/MonoMac.dll
MONOMAC_DLL_MDB_SOURCE=../../../../.config/MonoDevelop/addins/MonoDevelop.MonoMac.2.4.0.12/MonoMac.dll.mdb
MONOMAC_DLL_MDB=$(BUILD_DIR)/MonoMac.dll.mdb

endif

ifeq ($(CONFIG),RELEASE_IPHONESIMULATOR)
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize-
ASSEMBLY = bin/Release/CocosNetMac.exe
ASSEMBLY_MDB = 
COMPILE_TARGET = exe
PROJECT_REFERENCES = 
BUILD_DIR = bin/Release

COCOSNETMAC_EXE_MDB=
MONOMAC_DLL_SOURCE=../../../../.config/MonoDevelop/addins/MonoDevelop.MonoMac.2.4.0.12/MonoMac.dll
MONOMAC_DLL_MDB=

endif

ifeq ($(CONFIG),DEBUG_IPHONE)
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG"
ASSEMBLY = bin/Debug/CocosNetMac.exe
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = exe
PROJECT_REFERENCES = 
BUILD_DIR = bin/Debug

COCOSNETMAC_EXE_MDB_SOURCE=bin/Debug/CocosNetMac.exe.mdb
COCOSNETMAC_EXE_MDB=$(BUILD_DIR)/CocosNetMac.exe.mdb
MONOMAC_DLL_SOURCE=../../../../.config/MonoDevelop/addins/MonoDevelop.MonoMac.2.4.0.12/MonoMac.dll
MONOMAC_DLL_MDB_SOURCE=../../../../.config/MonoDevelop/addins/MonoDevelop.MonoMac.2.4.0.12/MonoMac.dll.mdb
MONOMAC_DLL_MDB=$(BUILD_DIR)/MonoMac.dll.mdb

endif

ifeq ($(CONFIG),RELEASE_IPHONE)
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize-
ASSEMBLY = bin/Release/CocosNetMac.exe
ASSEMBLY_MDB = 
COMPILE_TARGET = exe
PROJECT_REFERENCES = 
BUILD_DIR = bin/Release

COCOSNETMAC_EXE_MDB=
MONOMAC_DLL_SOURCE=../../../../.config/MonoDevelop/addins/MonoDevelop.MonoMac.2.4.0.12/MonoMac.dll
MONOMAC_DLL_MDB=

endif

AL=al2
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(COCOSNETMAC_EXE_MDB) \
	$(MONOMAC_DLL) \
	$(MONOMAC_DLL_MDB)  

BINARIES = \
	$(COCOSNETMAC)  


RESGEN=resgen2

MONOMAC_DLL = $(BUILD_DIR)/MonoMac.dll
COCOSNETMAC = $(BUILD_DIR)/cocosnetmac

FILES = \
	MainWindow.cs \
	MainWindowController.cs \
	MainWindow.xib.designer.cs \
	Main.cs \
	AppDelegate.cs \
	MainMenu.xib.designer.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	Info.plist \
	../../../opentk/Binaries/OpenTK/Release/OpenTK.dll \
	cocosnetmac.in 

REFERENCES =  \
	System \
	System.Xml \
	System.Core \
	System.Xml.Linq \
	System.Drawing \
	-pkg:monomac

DLL_REFERENCES = 

CLEANFILES = $(PROGRAMFILES) $(BINARIES) 

#Targets
all: $(ASSEMBLY) $(PROGRAMFILES) $(BINARIES)  $(top_srcdir)/config.make

include $(top_srcdir)/Makefile.include
#include $(srcdir)/custom-hooks.make



$(eval $(call emit-deploy-target,MONOMAC_DLL))
$(eval $(call emit-deploy-target,MONOMAC_DLL_MDB))
$(eval $(call emit-deploy-wrapper,COCOSNETMAC,cocosnetmac,x))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'


$(ASSEMBLY_MDB): $(ASSEMBLY)
$(ASSEMBLY): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	make pre-all-local-hook prefix=$(prefix)
	mkdir -p $(shell dirname $(ASSEMBLY))
	make $(CONFIG)_BeforeBuild
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
	make $(CONFIG)_AfterBuild
	make post-all-local-hook prefix=$(prefix)

install-local: $(ASSEMBLY) $(ASSEMBLY_MDB)
	make pre-install-local-hook prefix=$(prefix)
	make install-satellite-assemblies prefix=$(prefix)
	mkdir -p '$(DESTDIR)$(libdir)/$(PACKAGE)'
	$(call cp,$(ASSEMBLY),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(ASSEMBLY_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(COCOSNETMAC_EXE_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(MONOMAC_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(MONOMAC_DLL_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	mkdir -p '$(DESTDIR)$(bindir)'
	$(call cp,$(COCOSNETMAC),$(DESTDIR)$(bindir))
	make post-install-local-hook prefix=$(prefix)

uninstall-local: $(ASSEMBLY) $(ASSEMBLY_MDB)
	make pre-uninstall-local-hook prefix=$(prefix)
	make uninstall-satellite-assemblies prefix=$(prefix)
	$(call rm,$(ASSEMBLY),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(ASSEMBLY_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(COCOSNETMAC_EXE_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(MONOMAC_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(MONOMAC_DLL_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(COCOSNETMAC),$(DESTDIR)$(bindir))
	make post-uninstall-local-hook prefix=$(prefix)

install: install-local
uninstall: uninstall-local
clean: clean-local

include $(top_srcdir)/rules.make
